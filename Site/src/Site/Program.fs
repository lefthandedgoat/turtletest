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
open sausage_factory
open types.read
open types.permissions
open types.session
open types.response
open forms
open forms.newtypes

let register'' =
  choose [
    GET >>= warbler (fun _ ->
      OK register.html)
    POST >>= bindToForm newforms.newUser (fun newUser ->
      let errors = newvalidations.newUserValidation newUser
      if errors.Length > 0
      then OK <| register.error_html errors newUser
      else
        data.users.insert newUser |> ignore //ignore for now, in future return success/failure (duplicate username etc)
        FOUND <| paths.home_link newUser.Name)
  ]

let login'' =
  choose [
    GET >>= warbler (fun _ ->
      OK <| login.html false "")
    POST >>= bindToForm newforms.loginAttempt (fun loginAttempt ->
      let user = data.users.authenticate loginAttempt.Email loginAttempt.Password
      match user with
        | Some(u) ->
          Auth.authenticated Cookie.CookieLife.Session false
          >>= statefulForSession
          >>= sessionStore (fun store -> store.set "user_id" u.Id)
          >>= request (fun _ -> FOUND <| paths.home_link u.Name)
        | None -> OK <| login.html true loginAttempt.Email)
  ]

let logout'' = choose [ GET >>= reset ]

let home'' (user : User) session = warbler (fun _ ->
  let counts = data.fake.counts()
  let executions = data.fake.executions 5 ["Android"; "IOS"; "Desktop"]
  OK <| home.html user.Name counts executions)

let application'' id (user : User) session = warbler (fun _ ->
  let application = data.applications.tryById id
  match application with
    | None -> NOT_FOUND "Page not found"
    | Some(application) ->
      let counts = data.fake.counts()
      let permissions = data.permissions.getApplicationsCreateEditPermissions user.Name session
      let executions = data.fake.executions 8 [application.Name]
      let suites = data.suites.getByApplicationId application.Id
      OK <| applications.details permissions user.Name counts executions application suites)

let applicationCreate'' (user : User) session =
  let permissions = data.permissions.getApplicationsCreateEditPermissions user.Name session
  if ownerOrContributor permissions |> not
  then NOT_FOUND "Page not found"
  else
    let counts = data.fake.counts()
    choose [
      GET >>= warbler (fun _ ->
        OK <| applicationsCreate.html user.Name counts)
      POST >>= bindToForm newforms.newApplication (fun newApplication ->
        let errors = newvalidations.newApplicationValidation newApplication
        if errors.Length = 0
        then
          let id = data.applications.insert user.Id newApplication
          FOUND <| paths.application_link user.Name id
        else OK <| applicationsCreate.error_html user.Name counts errors newApplication)
    ]

let applicationEdit'' id (user : User) session =
  let permissions = data.permissions.getApplicationsCreateEditPermissions user.Name session
  if ownerOrContributor permissions |> not
  then NOT_FOUND "Page not found"
  else
    let counts = data.fake.counts()
    choose [
      GET >>= warbler (fun _ ->
        let application' = data.applications.tryById id
        match application' with
        | None -> NOT_FOUND "Page not found"
        | Some(application) ->
          OK <| applicationsEdit.html user.Name counts application)
      POST >>= bindToForm editforms.editApplication (fun editApplication ->
        let errors = editvalidations.editApplicationValidation editApplication
        if errors.Length = 0
        then
          data.applications.update id editApplication
          FOUND <| paths.application_link user.Name id
        else OK <| applicationsEdit.error_html user.Name counts errors editApplication)
    ]

let applications'' (user : User) (session : Session) = warbler (fun _ ->
  let permissions, apps = data.permissions.getPermissionsAndApplications user.Name session
  let counts = data.fake.counts()
  if ownerOrContributor permissions && apps.Length = 0
  then FOUND <| paths.applicationCreate_link user.Name
  else OK <| applications.list permissions user.Name counts apps)

let suite'' id (user : User) session = warbler (fun _ ->
  let suite = data.suites.tryById id
  match suite with
    | None -> NOT_FOUND "Page not found"
    | Some(suite') ->
      let counts = data.fake.counts()
      let testcases = data.fake.testcases
      let applications = data.applications.getByUserId user.Id
      OK <| suites.details user.Name suite' testcases applications counts)

let suiteCreate'' (user : User) session =
  let counts = data.fake.counts()
  let applications = data.applications.getByUserId user.Id
  choose [
    GET >>= warbler (fun _ ->
      OK <| suitesCreate.html user.Name counts applications)
    POST >>= bindToForm newforms.newSuite (fun newSuite ->
      let errors = newvalidations.newSuiteValidation newSuite
      if errors.Length > 0
      then OK <| suitesCreate.error_html user.Name counts applications errors newSuite
      else
        let application' = data.applications.tryById (int newSuite.Application)
        match application' with
        | None -> NOT_FOUND "Page not found"
        | Some(application) ->
          let id = data.suites.insert application.Id newSuite
          FOUND <| paths.suite_link user.Name id)
  ]

let suiteEdit'' id (user : User) session =
  let counts = data.fake.counts()
  let applications = data.applications.getByUserId user.Id
  choose [
    GET >>= warbler (fun _ ->
      let suite = data.suites.tryById id
      match suite with
      | None -> NOT_FOUND "Page not found"
      | Some(suite) ->
        OK <| suiteEdit.html user.Name counts applications suite)
    POST >>= bindToForm editforms.editSuite (fun editSuite ->
      let errors = editvalidations.editSuiteValidation editSuite
      if errors.Length > 0
      then OK <| suiteEdit.error_html user.Name counts errors applications editSuite
      else
        data.suites.update id editSuite
        FOUND <| paths.suite_link user.Name id)
  ]

let suites'' (user : User) session = warbler (fun _ ->
  let counts = data.fake.counts()
  let suites' = data.suites.getByUserId user.Id
  if suites'.Length = 0
  then FOUND <| paths.suiteCreate_link user.Name
  else OK <| suites.list user.Name counts suites')

let testcases'' (user : User) session = warbler (fun _ ->
  let counts = data.fake.counts()
  let testcase = data.fake.testcase
  OK <| testcases.html user.Name testcase counts)

let testcasesCreate'' (user : User) session =
  let counts = data.fake.counts()
  let applications = data.fake.applicationsOptions
  let suites = data.fake.suitesOptions
  choose [
    GET >>= warbler (fun _ ->
      OK <| testcasesCreate.html user.Name counts applications suites)
    //newSuiteValidation
    POST >>= bindToForm newforms.newTestCase (fun newTestCase ->
      let errors = newvalidations.newTestCaseValidation newTestCase
      if errors.Length > 0
      then OK <| testcasesCreate.error_html user.Name counts applications suites errors newTestCase
      else FOUND <| paths.testcases_link user.Name)
  ]

let executions'' (user : User) session = warbler (fun _ ->
  let counts = data.fake.counts()
  OK <| executions.html user.Name counts)

let root'' =
  choose [
    GET >>= (OK <| root.html Get)
    POST >>= bindToForm forms.email.interestedEmail (fun interestedEmail ->
      data.main_page_emails.insertEmail <| interestedEmail.Email.ToString()
      OK <| root.html Success)
  ]

let webPart =
  choose [
    path paths.root >>= root''
    path paths.login >>= login''
    path paths.register >>= register''
    path paths.logout >>= logout''
    pathScan paths.applicationCreate (fun userName -> userExists userName (canCreateEdit applicationCreate''))
    pathScan paths.applicationEdit (fun (userName, id) -> userExists userName (canCreateEdit (applicationEdit'' id)))
    pathScan paths.suiteCreate (fun userName -> userExists userName (canCreateEdit suiteCreate''))
    pathScan paths.suiteEdit (fun (userName, id) -> userExists userName (canCreateEdit (suiteEdit'' id)))
    pathScan paths.testcasesCreate (fun userName -> userExists userName (canCreateEdit testcasesCreate''))

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome

    GET >>= choose [
      pathScan paths.application (fun (userName, id) -> userExists userName (canView (application'' id)))
      pathScan paths.applications (fun userName -> userExists userName (canView applications''))
      pathScan paths.suite (fun (userName, id) -> userExists userName (canView (suite'' id)))
      pathScan paths.suites (fun userName -> userExists userName (canView suites''))
      pathScan paths.testcases (fun userName -> userExists userName (canView testcases''))
      pathScan paths.executions (fun userName -> userExists userName (canView executions''))
      pathScan paths.home (fun userName -> userExists userName (canView home''))
    ]
  ]

startWebServer defaultConfig webPart
