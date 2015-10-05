module html_bootstrap

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

let icon type' = italic ["class", (sprintf "fa fa-%s" type')] emptyText
let wrapper inner = divId "cl-wrapper" inner
let sidebar inner = divClass "cl-sidebar" inner
let toggle inner = divClass "cl-toggle" inner
let navblock inner = divClass "cl-navblock" inner
let menu_space inner = divClass "menu-space" inner
let content inner = divClass "content" inner
let mcontent inner = divClass "cl-mcont" inner
let sidebar_logo inner = divClass "sidebar-logo" inner
let logo inner = divClass "logo" inner
let vnavigation inner = ulAttr ["class", "cl-vnavigation"] inner
let row inner = divClass "row" inner
let m3sm6 inner = divClass "col-md-3 col-sm-6" inner
let tile color inner = divClass (sprintf "fd-tile detail clean tile-%s" color) inner

let side_link link text' count style icon' =
  li [
    aHref link [
      icon icon'
      span [text text']
      spanAttr ["class", sprintf "badge badge-%s pull-right" style] [text (string count)]
    ]
  ]

let left_sidebar =
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

let tileContainer link text' count color icon' =
  m3sm6 [
    tile color [
      content [
        h1Class "text-left" (text (string count))
        p (text text')
      ]
      divClass "icon" [ icon icon' ]
      aHrefAttr link ["class", "details"] [
        text "Details "
        span [ icon "arrow-circle-right pull-right" ]
      ]
    ]
  ]

let home_content =
  mcontent [
    row [
      tileContainer "/applications" "Applications" 4 "purple" "desktop"
      tileContainer "/suitse" "Suites" 19 "green" "sitemap"
      tileContainer "/testcases" "Test Cases" 143 "prusia" "thumbs-up"
      tileContainer "/executions" "Executions" 71 "red" "toggle-right"
    ]
    row [
    ]
  ]
