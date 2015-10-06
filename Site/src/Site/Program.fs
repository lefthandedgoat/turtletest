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

let home' = "/"
let applications' = "/applications"

let home'' = warbler (fun _ ->
  let counts = fake.counts()
  let executions = fake.executions 5 ["Android"; "IOS"; "Desktop"]
  let html' =
    html [
      head "home"
      body [
        wrapper [
          left_sidebar counts
          home_content counts executions
        ]
      ]
    ]
    |> xmlToString
  OK <| sprintf "<!DOCTYPE html>%s" html')

let applications'' = warbler (fun _ ->
  let counts = fake.counts()
  let executions = fake.executions 8 ["Android"]
  let html' =
    html [
      head "applications"
      body [
        wrapper [
          left_sidebar counts
          applications_content executions
        ]
      ]
    ]
    |> xmlToString
  OK <| sprintf "<!DOCTYPE html>%s" html')

let webPart =
  choose [

    GET >>= choose [
      path home' >>= home''
      path applications' >>= applications''
    ]

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome
  ]

startWebServer defaultConfig webPart
