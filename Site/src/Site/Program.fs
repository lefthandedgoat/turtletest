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

let home'' = warbler (fun _ ->
  let counts = fake.counts()
  let executions = fake.executions 5 ["Android"; "IOS"; "Desktop"]
  OK <| home.html counts executions)

let applications'' = warbler (fun _ ->
  let counts = fake.counts()
  let executions = fake.executions 8 ["Android"]
  OK <| applications.html counts executions)

let webPart =
  choose [

    GET >>= choose [
      path paths.home >>= home''
      path paths.applications >>= applications''
    ]

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome
  ]

startWebServer defaultConfig webPart
