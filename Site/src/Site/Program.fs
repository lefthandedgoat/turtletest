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

let home' = "/"

let home'' () =
  let html' =
    html [
      head
      body [
        wrapper [
          left_sidebar
          home_content
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'

let home = OK (home'' ())

let webPart =
  choose [

    GET >>= choose [
      path "/" >>= home
    ]

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome
  ]

startWebServer defaultConfig webPart
