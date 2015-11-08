module program

open System

open Suave
open Suave.Form
open Suave.Cookie
open Suave.Http
open Suave.Http.Redirection
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Http.Applicatives
open Suave.Model.Binding
open Suave.State.CookieStateStore
open Suave.Types
open Suave.Web
open Suave.Html
open html_common
open html_bootstrap
open types

let bindToForm form handler =
  //todo since we manually handle errors, make bad request log errors and send you too an oops page
  bindReq (bindForm form) handler BAD_REQUEST

let sessionStore setF = context (fun x ->
  match HttpContext.state x with
  | Some state -> setF state
  | None -> never)

let reset =
  unsetPair Auth.SessionAuthCookie
  >>= unsetPair StateCookie
  >>= FOUND paths.login

let getSession f =
  statefulForSession
  >>= context (fun x ->
    match x |> HttpContext.state with
    | None -> f NoSession
    | Some state ->
      match state.get "user_id" with
      | Some user_id -> f (User user_id)
      | _ -> f NoSession)

let redirectWithReturnPath redirection =
  request (fun x ->
    let path = x.url.AbsolutePath
    Redirection.FOUND (redirection |> paths.withParam ("returnPath", path)))

let loggedOn f_success =
  Auth.authenticate
    Cookie.CookieLife.Session
    false
    (fun () -> Choice2Of2(redirectWithReturnPath paths.login))
    (fun _ -> Choice2Of2 reset)
    f_success

let canCreateEdit f_success =
  loggedOn (getSession (fun session ->
    match session with
    | User(_) -> f_success session
    | _ -> UNAUTHORIZED "Not logged in"))

let canView f_success =
  getSession (fun sess -> f_success sess)

let register'' =
  choose [
    GET >>= warbler (fun _ ->
      OK register.html)
    POST >>= bindToForm forms.newUser (fun newUser ->
      let errors = forms.newUserValidation newUser
      if errors.Length > 0
      then OK <| register.error_html errors newUser
      else
        data_users.insert newUser |> ignore //ignore for now, in future return success/failure (duplicate username etc)
        FOUND <| paths.home_link newUser.Name)
  ]

let login'' =
  choose [
    GET >>= warbler (fun _ ->
      OK <| login.html false "")
    POST >>= bindToForm forms.loginAttempt (fun loginAttempt ->
      let user = data_users.authenticate loginAttempt.Email loginAttempt.Password
      match user with
        | Some(u) ->
          Auth.authenticated Cookie.CookieLife.Session false
          >>= statefulForSession
          >>= sessionStore (fun store -> store.set "user_id" u.Id)
          >>= request (fun _ -> FOUND <| paths.home_link u.Name)
        | None -> OK <| login.html true loginAttempt.Email)
  ]

let logout'' = choose [ GET >>= reset ]

let home'' userName = warbler (fun _ ->
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(_) ->
      let counts = fake.counts()
      let executions = fake.executions 5 ["Android"; "IOS"; "Desktop"]
      OK <| home.html userName counts executions)

let application'' (userName, id) session = warbler (fun _ ->
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(_) ->
      let application = data_applications.tryById id
      match application with
        | None -> NOT_FOUND "Page not found"
        | Some(application) ->
          let counts = fake.counts()
          let permissions = data_permissions.getApplicationsCreateEditPermissions userName session
          let executions = fake.executions 8 [application.Name]
          let suites = data_suites.getByApplicationId application.Id
          OK <| applications.details permissions userName counts executions application suites)

let applicationCreate'' userName session =
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(user) ->
      let permissions = data_permissions.getApplicationsCreateEditPermissions userName session
      if ownerOrContributor permissions |> not
      then NOT_FOUND "Page not found"
      else
        let counts = fake.counts()
        choose [
          GET >>= warbler (fun _ ->
            OK <| applicationsCreate.html userName counts)
          POST >>= bindToForm forms.newApplication (fun newApplication ->
            let errors = forms.newApplicationValidation newApplication
            if errors.Length = 0
            then
              let id = data_applications.insert user.Id newApplication
              FOUND <| paths.application_link userName id
            else OK <| applicationsCreate.error_html userName counts errors newApplication)
        ]

let applicationEdit'' (userName, id) session =
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(_) ->
      let permissions = data_permissions.getApplicationsCreateEditPermissions userName session
      if ownerOrContributor permissions |> not
      then NOT_FOUND "Page not found"
      else
        let counts = fake.counts()
        choose [
          GET >>= warbler (fun _ ->
            let application' = data_applications.tryById id
            match application' with
            | None -> NOT_FOUND "Page not found"
            | Some(application) ->
              OK <| applicationsEdit.html userName counts application)
          POST >>= bindToForm forms.editApplication (fun editApplication ->
            let errors = forms.editApplicationValidation editApplication
            if errors.Length = 0
            then
              data_applications.update id editApplication
              FOUND <| paths.application_link userName id
            else OK <| applicationsEdit.error_html userName counts errors editApplication)
        ]

let applications'' userName (session : Session) = warbler (fun _ ->
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(_) ->
      let permissions, apps = data_permissions.getPermissionsAndApplications userName session
      let counts = fake.counts()
      if ownerOrContributor permissions && apps.Length = 0
      then FOUND <| paths.applicationCreate_link userName
      else OK <| applications.list permissions userName counts apps)

let suite'' (userName, id) session = warbler (fun _ ->
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(user) ->
      let suite = data_suites.tryById id
      match suite with
        | None -> NOT_FOUND "Page not found"
        | Some(suite') ->
          let counts = fake.counts()
          let testcases = fake.testcases
          let applications = data_applications.getByUserId user.Id
          OK <| suites.details userName suite' testcases applications counts)

let suiteCreate'' userName session =
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(user) ->
      let counts = fake.counts()
      let applications = data_applications.getByUserId user.Id
      choose [
        GET >>= warbler (fun _ ->
          OK <| suitesCreate.html userName counts applications)
        POST >>= bindToForm forms.newSuite (fun newSuite ->
          let errors = forms.newSuiteValidation newSuite
          if errors.Length > 0
          then OK <| suitesCreate.error_html userName counts applications errors newSuite
          else
            let application' = data_applications.tryById (int newSuite.Application)
            match application' with
            | None -> NOT_FOUND "Page not found"
            | Some(application) ->
              let id = data_suites.insert application.Id newSuite
              FOUND <| paths.suite_link userName id)
      ]

let suiteEdit'' (userName, id) session =
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(user) ->
      let counts = fake.counts()
      let applications = data_applications.getByUserId user.Id
      choose [
        GET >>= warbler (fun _ ->
          let suite = data_suites.tryById id
          match suite with
          | None -> NOT_FOUND "Page not found"
          | Some(suite) ->
            OK <| suiteEdit.html userName counts applications suite)
        POST >>= bindToForm forms.editSuite (fun editSuite ->
          let errors = forms.editSuiteValidation editSuite
          if errors.Length > 0
          then OK <| suiteEdit.error_html userName counts errors applications editSuite
          else
            data_suites.update id editSuite
            FOUND <| paths.suite_link userName id)
      ]

let suites'' userName session = warbler (fun _ ->
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(user) ->
      let counts = fake.counts()
      let suites' = data_suites.getByUserId user.Id
      if suites'.Length = 0
      then FOUND <| paths.suiteCreate_link userName
      else OK <| suites.list userName counts suites')

let testcases'' userName session = warbler (fun _ ->
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(user) ->
      let counts = fake.counts()
      let testcase = fake.testcase
      OK <| testcases.html userName testcase counts)

let testcasesCreate'' userName session =
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(user) ->
      let counts = fake.counts()
      let applications = fake.applicationsOptions
      let suites = fake.suitesOptions
      choose [
        GET >>= warbler (fun _ ->
          OK <| testcasesCreate.html userName counts applications suites)
        //newSuiteValidation
        POST >>= bindToForm forms.newTestCase (fun newTestCase ->
          let errors = forms.newTestCaseValidation newTestCase
          if errors.Length > 0
          then OK <| testcasesCreate.error_html userName counts applications suites errors newTestCase
          else FOUND <| paths.testcases_link userName)
      ]

let executions'' userName = warbler (fun _ ->
  let user = data_users.tryByName userName
  match user with
    | None -> NOT_FOUND "Page not found"
    | Some(user) ->
      let counts = fake.counts()
      OK <| executions.html userName counts)

let root'' =
  choose [
    GET >>= (OK <| root.html Get)
    POST >>= bindToForm forms.interestedEmail (fun interestedEmail ->
      main_page_emails.insertEmail <| interestedEmail.Email.ToString()
      OK <| root.html Success)
  ]

let webPart =
  choose [
    path paths.root >>= root''
    path paths.login >>= login''
    path paths.register >>= register''
    path paths.logout >>= logout''
    pathScan paths.applicationCreate (fun userName -> canCreateEdit (applicationCreate'' userName))
    pathScan paths.applicationEdit (fun (userName, id) -> canCreateEdit (applicationEdit'' (userName, id)))
    pathScan paths.suiteCreate (fun userName -> canCreateEdit (suiteCreate'' userName))
    pathScan paths.suiteEdit (fun (userName, id) -> canCreateEdit (suiteEdit'' (userName, id)))
    pathScan paths.testcasesCreate (fun userName -> canCreateEdit (testcasesCreate'' userName))

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome

    GET >>= choose [
      pathScan paths.application (fun (userName, id) -> canView (application'' (userName,id)))
      pathScan paths.applications (fun userName -> canView (applications'' userName))
      pathScan paths.suite (fun (userName, id) -> canView (suite'' (userName,id)))
      pathScan paths.suites (fun userName -> canView (suites'' userName))
      pathScan paths.testcases (fun userName -> canView (testcases'' userName))
      pathScan paths.executions executions''
      pathScan paths.home home''
    ]
  ]

startWebServer defaultConfig webPart
