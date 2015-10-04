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
  html [
    head
    body [
      wrapper [
        sidebar [
          toggle [ icon "bars" ]
          navblock [
            menu_space [
              content [
                sidebar_logo [
                  logo [ aHref "index2.html" [] ]
                ]
                vnavigation [
                  side_link "/applications" "Applications" 4 "primary" "desktop"
                  side_link "/suites" "Suites" 19 "success" "sitemap"
                  side_link "/testcases" "Test Cases" 143 "prusia" "thumbs-up"
                  side_link "/executions" "Executions" 71 "danger" "toggle-right"
                ]
              ]
            ]
          ]
        ]
      ]
    ]
  ]
  |> xmlToString

let home = OK (home'' ())

let webPart =
  choose [

    GET >>= choose [
      path "/" >>= home
    ]

    pathRegex "(.*)\.(css|png|gif|js|ico|woff|tff)" >>= Files.browseHome
  ]

startWebServer defaultConfig webPart
