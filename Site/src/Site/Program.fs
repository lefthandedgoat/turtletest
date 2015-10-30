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
    bindReq (bindForm form) handler BAD_REQUEST

let home'' user = warbler (fun _ ->
  let user' = data_users.tryByName user
  match user' with
    | Some(user) ->
      let counts = fake.counts()
      let executions = fake.executions 5 ["Android"; "IOS"; "Desktop"]
      OK <| home.html user.Name counts executions
    | None -> Suave.Http.RequestErrors.NOT_FOUND "Page not found")

let application'' (user, id) = warbler (fun _ ->
  let counts = fake.counts()
  let application = data_applications.getById id
  let executions = fake.executions 8 [application.Name]
  let suites = fake.suites
  OK <| applications.html user counts executions application suites)

let applicationCreate'' user =
  let counts = fake.counts()
  choose [
    GET >>= warbler (fun _ ->
      OK <| applicationsCreate.html user counts)
    POST >>= bindToForm forms.newApplication (fun form ->
      printfn "%A" form
      let id = data_applications.insert form
      FOUND <| paths.application_link user id)
  ]

let suites'' user = warbler (fun _ ->
  let counts = fake.counts()
  let suite = fake.suite
  let testcases = fake.testcases
  OK <| suites.html user suite testcases counts)

let suitesCreate'' user =
  let counts = fake.counts()
  let applications = fake.applicationsOptions
  choose [
    GET >>= warbler (fun _ ->
      OK <| suitesCreate.html user counts applications)
    POST >>= bindToForm forms.newSuite (fun form ->
      printfn "%A" form
      FOUND <| paths.suites_link user)
  ]

let testcases'' user = warbler (fun _ ->
  let counts = fake.counts()
  let testcase = fake.testcase
  OK <| testcases.html user testcase counts)

let testcasesCreate'' user =
  let counts = fake.counts()
  let applications = fake.applicationsOptions
  let suites = fake.suitesOptions
  choose [
    GET >>= warbler (fun _ ->
      OK <| testcasesCreate.html user counts applications suites)
    POST >>= bindToForm forms.newTestCase (fun form ->
      printfn "%A" form
      FOUND <| paths.testcases_link user)
  ]

let executions'' user = warbler (fun _ ->
  let counts = fake.counts()
  OK <| executions.html user counts)

let root'' =
  choose [
    GET >>= (OK <| root.html Get)
    POST >>= bindToForm forms.interestedEmail (fun form ->
      main_page_emails.insertEmail <| form.Email.ToString()
      OK <| root.html Success)
  ]

let webPart =
  choose [
    path paths.root >>= root''
    pathScan paths.applicationCreate applicationCreate''
    pathScan paths.suitesCreate suitesCreate''
    pathScan paths.testcasesCreate testcasesCreate''

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome

    GET >>= choose [
      pathScan paths.application application''
      pathScan paths.suites suites''
      pathScan paths.testcases testcases''
      pathScan paths.executions executions''
      pathScan paths.home home''
    ]
  ]

startWebServer defaultConfig webPart
