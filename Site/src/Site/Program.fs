module program

open System

open Suave
open Suave.Form
open Suave.Cookie
open Suave.Http
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

let home'' user = warbler (fun _ ->
  let counts = fake.counts()
  let executions = fake.executions 5 ["Android"; "IOS"; "Desktop"]
  OK <| home.html user counts executions)

let applications'' user = warbler (fun _ ->
  let counts = fake.counts()
  let executions = fake.executions 8 ["Android"]
  let application = fake.application
  let suites = fake.suites
  OK <| applications.html user counts executions application suites)

let suites'' user = warbler (fun _ ->
  let counts = fake.counts()
  OK <| suites.html user counts)

let testcases'' user = warbler (fun _ ->
  let counts = fake.counts()
  OK <| testcases.html user counts)

let executions'' user = warbler (fun _ ->
  let counts = fake.counts()
  OK <| executions.html user counts)

let bindToForm form handler =
    bindReq (bindForm form) handler BAD_REQUEST

let root'' =
  choose [
    GET >>= (OK <| root.html)
    POST >>= bindToForm forms.interestedEmail (fun form ->
      main_page_emails.insertEmail <| form.Email.ToString()
      OK <| root.html)
  ]

let webPart =
  choose [
    path paths.root >>= root''

    GET >>= choose [
      pathScan paths.home home''
      pathScan paths.applications applications''
      pathScan paths.suites suites''
      pathScan paths.testcases testcases''
      pathScan paths.executions executions''
    ]

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome
  ]

startWebServer defaultConfig webPart
