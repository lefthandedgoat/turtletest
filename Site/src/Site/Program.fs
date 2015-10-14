module program

open System

open Suave
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

let main_page_email'' email = warbler (fun _ ->
  main_page_emails.insertEmail email
  OK <| "")


let root'' = warbler (fun _ ->
  OK <| root.html)

let webPart =
  choose [
    GET >>= choose [
      pathScan paths.home home''
      pathScan paths.applications applications''
      pathScan paths.suites suites''
      pathScan paths.testcases testcases''
      pathScan paths.executions executions''
      pathScan paths.main_page_email main_page_email''
      path     paths.root >>= root''
    ]

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome
  ]

startWebServer defaultConfig webPart
