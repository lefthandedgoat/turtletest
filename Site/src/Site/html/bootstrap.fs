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

let home_content =
  mcontent [
    row [
      tileContainer "/applications" "Applications" 4 "purple" "desktop"
      tileContainer "/suitse" "Suites" 19 "green" "sitemap"
      tileContainer "/testcases" "Test Cases" 143 "prusia" "thumbs-up"
      tileContainer "/executions" "Executions" 71 "red" "toggle-right"
    ]
    row [
      m12 [
        block_flat [
          header [ h3 "Recent Executions" ]
          content [
            table_responsive [
              tableClass "no-border hover list" [
                tbodyClass "no-border-y" [
                  executionRow "Android" "Week 14 Regression" 80
                  executionRow "IOS" "JIRA #1043 critical bug fix for crash" 33
                  executionRow "Website" "Sprint 32 Automated regression QA4 Environment" 59
                  executionRow "Android" "Week 13 Regression" 93
                  executionRow "Android" "Week 12 Regression" 55
                ]
              ]
            ]
          ]
        ]
      ]
    ]
  ]

let applications_content =
  mcontent [
    row [
      m12 [
        block_flat [
          header [ h3 "Recent Executions" ]
          content [
            table_responsive [
              tableClass "no-border hover list" [
                tbodyClass "no-border-y" [
                  executionRow "Android" "Week 14 Regression" 80
                  executionRow "Android" "JIRA #1043 critical bug fix for crash" 33
                  executionRow "Android" "Sprint 32 Automated regression QA4 Environment" 59
                  executionRow "Android" "Week 13 Regression" 93
                  executionRow "Android" "Week 12 Regression" 55
                ]
              ]
            ]
          ]
        ]
      ]
    ]
  ]
