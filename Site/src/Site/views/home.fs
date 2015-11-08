module home

open Suave.Html
open html_common
open html_bootstrap
open partial_tile
open partial_executions
open types.read

let home_content user counts executionRows =
  mcontent [
    row [
      tileContainer (paths.applications_link user) "Applications" counts.Applications "purple" "desktop"
      tileContainer (paths.suites_link user) "Suites" counts.Suites "green" "sitemap"
      tileContainer (paths.testcases_link user) "Test Cases" counts.TestCases "prusia" "thumbs-up"
      tileContainer (paths.executions_link user) "Executions" counts.Executions "red" "toggle-right"
    ]
    row [
      m12 [
        execution executionRows
      ]
    ]
  ]

let html user counts executions =
  base_html
    "home"
    [
      partial_sidebar.left_sidebar user counts
      home_content user counts executions
    ]
    scripts.none
