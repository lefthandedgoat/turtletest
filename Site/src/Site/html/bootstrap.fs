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
open types

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
let m12 inner = divClass "col-md-12" inner
let block_flat inner = divClass "block-flat" inner
let header inner = divClass "header" inner
let labelX x inner = spanAttr ["class", (sprintf "label label-%s" x)] [inner]
let tdColor color inner = tdAttr ["class", (sprintf "color-%s" color)] inner
let table_responsive inner = divClass "table-responsive" inner
let progress_bar type' percent inner = divAttr ["class", (sprintf "progress-bar progress-bar-%s") type'; "style",(sprintf "width: %s" percent) ] [inner]
let tile color inner = divClass (sprintf "fd-tile detail clean tile-%s" color) inner
let sidebar_item inner = spanAttr ["class","sidebar-item"] inner

let side_link link text' count style icon' =
  li [
    aHref link [
      icon icon'
      sidebar_item [text text']
      spanAttr ["class", sprintf "badge badge-%s pull-right" style] [text (string count)]
    ]
  ]

let left_sidebar counts =
  sidebar [
    toggle [ icon "bars" ]
    navblock [
      menu_space [
        content [
          sidebar_logo [
            logo [ aHref "index2.html" [] ]
          ]
          vnavigation [
            side_link "/applications" "Applications" counts.Applications "primary" "desktop"
            side_link "/suites" "Suites" counts.Suites "success" "sitemap"
            side_link "/testcases" "Test Cases" counts.TestCases "prusia" "thumbs-up"
            side_link "/executions" "Executions" counts.Executions "danger" "toggle-right"
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
        p [ (text text') ]
      ]
      divClass "icon" [ icon icon' ]
      aHrefAttr link ["class", "details"] [
        text "Details "
        span [ icon "arrow-circle-right pull-right" ]
      ]
    ]
  ]

let executionRow suite name percent =
  let color =
    if percent >= 75 then "success"
    else if percent >= 50 then "warning"
    else "danger"
  let percent = (string percent) + "%"

  trClass "items" [
    tdAttr [ "style","width: 10%;" ] [ labelX color (text "Execution") ]
    td [ p [ strong suite ] ]
    td [ p [ span [(text name) ] ] ]
    tdColor color [ divClass "progress" [ progress_bar color percent (text percent) ]]
  ]

let home_content counts executionRows =
  mcontent [
    row [
      tileContainer "/applications" "Applications" counts.Applications "purple" "desktop"
      tileContainer "/suites" "Suites" counts.Suites "green" "sitemap"
      tileContainer "/testcases" "Test Cases" counts.TestCases "prusia" "thumbs-up"
      tileContainer "/executions" "Executions" counts.Executions "red" "toggle-right"
    ]
    row [
      m12 [
        block_flat [
          header [ h3 "Recent Executions" ]
          content [
            table_responsive [
              tableClass "no-border hover list" [
                tbodyClass "no-border-y" (executionRows |> List.map (fun er -> executionRow er.Application er.Description er.Percent))
              ]
            ]
          ]
        ]
      ]
    ]
  ]

let applications_content executionRows =
  mcontent [
    row [
      m12 [
        block_flat [
          header [ h3 "Recent Executions" ]
          content [
            table_responsive [
              tableClass "no-border hover list" [
                tbodyClass "no-border-y" (executionRows |> List.map (fun er -> executionRow er.Application er.Description er.Percent))
              ]
            ]
          ]
        ]
      ]
    ]
  ]
