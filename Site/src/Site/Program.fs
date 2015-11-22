module program

open System
open Suave
open Suave.Http
open Suave.Http.Redirection
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Http.Applicatives
open Suave.State.CookieStateStore
open Suave.Types
open Suave.Web
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
      OK views.register.html)
    POST >>= bindToForm newforms.newUser (fun newUser ->
      let errors = newvalidations.newUserValidation newUser
      if errors.Length > 0
      then OK <| views.register.error_html errors newUser
      else
        //todo check for duplicates etc, I think it will error
        let id = data.users.insert newUser
        //todo common code to move to the sausage factory???
        Auth.authenticated Cookie.CookieLife.Session false
        >>= statefulForSession
        >>= sessionStore (fun store -> store.set "user_id" id)
        >>= request (fun _ -> FOUND <| paths.home_link newUser.Name))
  ]

let login'' =
  choose [
    GET >>= warbler (fun _ ->
      OK <| views.login.html false "")
    POST >>= bindToForm newforms.loginAttempt (fun loginAttempt ->
      let user = data.users.authenticate loginAttempt.Email loginAttempt.Password
      match user with
        | Some(u) ->
          Auth.authenticated Cookie.CookieLife.Session false
          >>= statefulForSession
          >>= sessionStore (fun store -> store.set "user_id" u.Id)
          >>= request (fun _ -> FOUND <| paths.home_link u.Name)
        | None -> OK <| views.login.html true loginAttempt.Email)
  ]

let logout'' = choose [ GET >>= reset ]

let home'' (user : User) session = warbler (fun _ ->
  let counts = data.counts.getCounts user.Name session
  let testruns = data.fake.testruns 5 ["Android"; "IOS"; "Desktop"]
  OK <| views.home.html session user.Name counts testruns)

let application'' (application : Application) (user : User) permissions session = warbler (fun _ ->
  let counts = data.counts.getCounts user.Name session
  let testruns = data.fake.testruns 8 [application.Name]
  let suites = data.suites.getByApplicationId application.Id
  OK <| views.applications.details session permissions user.Name counts testruns application suites)

let applicationCreate'' (user : User) session =
  let permissions = data.permissions.getApplicationsCreateEditPermissions user.Name session
  if owner permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    choose [
      GET >>= warbler (fun _ ->
        OK <| views.applicationsCreate.html session user.Name counts)
      POST >>= bindToForm newforms.newApplication (fun newApplication ->
        let errors = newvalidations.newApplicationValidation newApplication
        if errors.Length = 0
        then
          let id = data.applications.insert user.Id newApplication
          data.permissions.insert user.Id id Permissions.Owner
          FOUND <| paths.application_link user.Name id
        else OK <| views.applicationsCreate.error_html session user.Name counts errors newApplication)
    ]

let applicationEdit'' id (user : User) session =
  let permissions = data.permissions.getApplicationsCreateEditPermissions user.Name session
  if owner permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    choose [
      GET >>= warbler (fun _ ->
        let application' = data.applications.tryById id
        match application' with
        | None -> OK views.errors.error_404
        | Some(application) ->
          OK <| views.applicationsEdit.html session user.Name counts application)
      POST >>= bindToForm editforms.editApplication (fun editApplication ->
        let errors = editvalidations.editApplicationValidation editApplication
        if errors.Length = 0
        then
          data.applications.update id editApplication
          FOUND <| paths.application_link user.Name id
        else OK <| views.applicationsEdit.error_html session user.Name counts errors editApplication)
    ]

let applications'' (user : User) (session : Session) = warbler (fun _ ->
  let permissions, apps = data.permissions.getPermissionsAndApplications user.Name session
  let counts = data.counts.getCounts user.Name session
  if ownerOrContributor permissions && apps.Length = 0
  then FOUND <| paths.applicationCreate_link user.Name
  else OK <| views.applications.list session permissions user.Name counts apps)

let suite'' id (user : User) session = warbler (fun _ ->
  let permissions = data.permissions.getSpecificSuiteCreateEditPermissions id session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let suite = data.suites.tryById id
    match suite with
      | None -> OK views.errors.error_404
      | Some(suite') ->
        let counts = data.counts.getCounts user.Name session
        let testcases = data.testcases.getBySuiteId id
        let applications = data.applications.getByUserId user.Id
        OK <| views.suites.details session user.Name suite' testcases applications counts)

let suiteCreate'' (user : User) session =
  let permissions = data.permissions.getSuiteCreateEditPermissions user.Name session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    let applications = data.applications.getByUserId user.Id
    choose [
      GET >>= request (fun req ->
        let applicationId = getQueryStringValue req "applicationId"
        OK <| views.suitesCreate.html session user.Name counts applications applicationId)
      POST >>= bindToForm newforms.newSuite (fun newSuite ->
        let errors = newvalidations.newSuiteValidation newSuite
        if errors.Length > 0
        then OK <| views.suitesCreate.error_html session user.Name counts applications errors newSuite
        else
          let application' = data.applications.tryById (int newSuite.Application)
          match application' with
          | None -> OK views.errors.error_404
          | Some(application) ->
            let id = data.suites.insert application.Id newSuite
            FOUND <| paths.suite_link user.Name id)
    ]

let suiteEdit'' id (user : User) session =
  let permissions = data.permissions.getSpecificSuiteCreateEditPermissions id session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    let applications = data.applications.getByUserId user.Id
    choose [
      GET >>= warbler (fun _ ->
        let suite = data.suites.tryById id
        match suite with
        | None -> OK views.errors.error_404
        | Some(suite) ->
          OK <| views.suiteEdit.html session user.Name counts applications suite)
      POST >>= bindToForm editforms.editSuite (fun editSuite ->
        let errors = editvalidations.editSuiteValidation editSuite
        if errors.Length > 0
        then OK <| views.suiteEdit.error_html session user.Name counts errors applications editSuite
        else
          data.suites.update id editSuite
          FOUND <| paths.suite_link user.Name id)
    ]

let suites'' (user : User) session = warbler (fun _ ->
  //todo switch to being able to view public suites
  let permissions = data.permissions.getSuiteCreateEditPermissions user.Name session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    let suites' = data.suites.getByUserId user.Id
    if suites'.Length = 0
    then FOUND <| paths.suiteCreate_link user.Name
    else OK <| views.suites.list session user.Name counts suites')

let testcase'' id (user : User) session = warbler (fun _ ->
  let testcase = data.testcases.tryById id
  match testcase with
    | None -> OK views.errors.error_404
    | Some(testcase) ->
      //todo switch to being able to view public test cases
      let permissions = data.permissions.getSpecificTestCaseCreateEditPermissions id session
      if ownerOrContributor permissions |> not
      then OK views.errors.error_404
      else
        let counts = data.counts.getCounts user.Name session
        let applications = data.applications.getByUserId user.Id
        let suites = data.suites.getByUserId user.Id
        OK <| views.testcases.details session permissions user.Name applications suites counts testcase)

let testcaseCreate'' (user : User) session =
  let permissions = data.permissions.getTestCaseCreateEditPermissions user.Name session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    let applications = data.applications.getByUserId user.Id
    let suites = data.suites.getByUserId user.Id
    choose [
      GET >>= request (fun req ->
        let applicationId = getQueryStringValue req "applicationId"
        let suiteId = getQueryStringValue req "suiteId"
        OK <| views.testcasesCreate.html session user.Name counts applications suites applicationId suiteId)
      POST >>= bindToForm newforms.newTestCase (fun newTestCase ->
        let errors = newvalidations.newTestCaseValidation newTestCase
        if errors.Length = 0
        then
          let id = data.testcases.insert newTestCase
          FOUND <| paths.testcase_link user.Name id
        else OK <| views.testcasesCreate.error_html session user.Name counts applications suites errors newTestCase)
    ]

let testcaseEdit'' id (user : User) session =
  let permissions = data.permissions.getSpecificTestCaseCreateEditPermissions id session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    let applications = data.applications.getByUserId user.Id
    let suites = data.suites.getByUserId user.Id
    choose [
      GET >>= warbler (fun _ ->
        let testcase = data.testcases.tryById id
        match testcase with
        | None -> OK views.errors.error_404
        | Some(testcase) ->
          OK <| views.testcaseEdit.html session user.Name counts applications suites testcase)
      POST >>= bindToForm editforms.editTestCase (fun editTestCase ->
        let errors = editvalidations.editTestCaseValidation editTestCase
        if errors.Length > 0
        then OK <| views.testcaseEdit.error_html session user.Name counts errors applications suites editTestCase
        else
          data.testcases.update id editTestCase
          FOUND <| paths.testcase_link user.Name id)
    ]

let testcases'' (user : User) session = warbler (fun _ ->
  //todo switch to being able to view public test cases
  let permissions = data.permissions.getTestCaseCreateEditPermissions user.Name session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    let testcases = data.testcases.getByUserId user.Id
    if testcases.Length = 0
    then FOUND <| paths.testcaseCreate_link user.Name
    else
      OK <| views.testcases.list session permissions user.Name counts testcases)

let testrun'' id (user : User) session = warbler (fun _ ->
  let testrun = data.testruns.tryById id
  match testrun with
    | None -> OK views.errors.error_404
    | Some(testrun) ->
      //todo switch to being able to view public test runs
      let permissions = data.permissions.getSpecificTestRunCreateEditPermissions id session
      if ownerOrContributor permissions |> not
      then OK views.errors.error_404
      else
        let counts = data.counts.getCounts user.Name session
        let applications = data.applications.getByUserId user.Id
        OK <| views.testruns.details session permissions user.Name applications counts testrun)

let testrunCreate'' (user : User) session =
  let permissions = data.permissions.getTestRunCreateEditPermissions user.Name session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    let applications = data.applications.getByUserId user.Id
    choose [
      GET >>= request (fun _ -> //todo back to req from _
        //let applicationId = getQueryStringValue req "applicationId" //todo add this back
        OK <| views.testrunsCreate.html session user.Name counts applications)
      POST >>= bindToForm newforms.newTestRun (fun newTestRun ->
        let errors = newvalidations.newTestRunValidation newTestRun
        if errors.Length = 0
        then
          let id = data.testruns.insert newTestRun
          FOUND <| paths.testrun_link user.Name id
        else OK <| views.testrunsCreate.error_html session user.Name counts applications errors newTestRun)
    ]

let testrunEdit'' id (user : User) session =
  let permissions = data.permissions.getSpecificTestRunCreateEditPermissions id session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    let applications = data.applications.getByUserId user.Id
    choose [
      GET >>= warbler (fun _ ->
        let testrun = data.testruns.tryById id
        match testrun with
        | None -> OK views.errors.error_404
        | Some(testrun) ->
          OK <| views.testrunEdit.html session user.Name counts applications testrun)
      POST >>= bindToForm editforms.editTestRun (fun editTestRun ->
        let errors = editvalidations.editTestRunValidation editTestRun
        if errors.Length > 0
        then OK <| views.testrunEdit.error_html session user.Name counts errors applications editTestRun
        else
          data.testruns.update id editTestRun
          FOUND <| paths.testrun_link user.Name id)
    ]

let testruns'' (user : User) session = warbler (fun _ ->
  //todo switch to being able to view public test runs
  let permissions = data.permissions.getTestRunCreateEditPermissions user.Name session
  if ownerOrContributor permissions |> not
  then OK views.errors.error_404
  else
    let counts = data.counts.getCounts user.Name session
    let testruns = data.testruns.getByUserId user.Id
    OK <| views.testruns.list session permissions user.Name counts testruns)

let root'' =
  choose [
    GET >>= (OK <| views.root.html Get)
    POST >>= bindToForm forms.email.interestedEmail (fun interestedEmail ->
      data.main_page_emails.insertEmail <| interestedEmail.Email.ToString()
      OK <| views.root.html Success)
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

    pathScan paths.testcaseCreate (fun userName -> userExists userName (canCreateEdit testcaseCreate''))
    pathScan paths.testcaseEdit (fun (userName, id) -> userExists userName (canCreateEdit (testcaseEdit'' id)))

    pathScan paths.testrunCreate (fun userName -> userExists userName (canCreateEdit testrunCreate''))
    pathScan paths.testrunEdit (fun (userName, id) -> userExists userName (canCreateEdit (testrunEdit'' id)))

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome

    GET >>= choose [
      pathScan paths.application (fun (userName, id) -> userExists userName (canView (applicationExistsAndUserHasPermissions id application'')))
      pathScan paths.applications (fun userName -> userExists userName (canView applications''))

      pathScan paths.suite (fun (userName, id) -> userExists userName (canView (suite'' id)))
      pathScan paths.suites (fun userName -> userExists userName (canView suites''))

      pathScan paths.testcase (fun (userName, id) -> userExists userName (canView (testcase'' id)))
      pathScan paths.testcases (fun userName -> userExists userName (canView testcases''))

      pathScan paths.testrun (fun (userName, id) -> userExists userName (canView (testrun'' id)))
      pathScan paths.testruns (fun userName -> userExists userName (canView testruns''))

      pathScan paths.home (fun userName -> userExists userName (canView home''))
    ]
  ]

//todo put this in a web.config or something
let config = { defaultConfig with
                 errorHandler = sausage_factory.errorHandler
                 serverKey = Text.Encoding.Default.GetBytes("""806970382358F417C7610E866FE2598B""")
             }
startWebServer config webPart
