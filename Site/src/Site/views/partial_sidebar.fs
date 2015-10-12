module partial_sidebar

open Suave.Html
open html_common
open html_bootstrap
open types

let private side_link link text' count style icon' =
  li [
    aHref link [
      icon icon'
      sidebar_item [text text']
      spanAttr ["class", sprintf "badge badge-%s pull-right" style] [text (string count)]
    ]
  ]

let left_sidebar user counts =
  sidebar [
    toggle [ icon "bars" ]
    navblock [
      menu_space [
        content [
          aHref (paths.home_link user) [
            sidebar_logo [
              logo [ emptyText ]
            ]
          ]
          vnavigation [
            side_link (paths.applications_link user) "Applications" counts.Applications "primary" "desktop"
            side_link (paths.suites_link user) "Suites" counts.Suites "success" "sitemap"
            side_link (paths.testcases_link user) "Test Cases" counts.TestCases "prusia" "thumbs-up"
            side_link (paths.executions_link user) "Executions" counts.Executions "danger" "toggle-right"
          ]
        ]
      ]
    ]
  ]
