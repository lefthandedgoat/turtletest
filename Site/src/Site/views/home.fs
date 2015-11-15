module views.home

open Suave.Html
open html_common
open html_bootstrap
open views.partial
open types.read

let home_content user counts testrunRows =
  mcontent [
    row [
      partial.tile.tileContainer (paths.applications_link user) "Applications" counts.Applications "purple" "desktop"
      partial.tile.tileContainer (paths.suites_link user) "Suites" counts.Suites "green" "sitemap"
      partial.tile.tileContainer (paths.testcases_link user) "Test Cases" counts.TestCases "prusia" "thumbs-up"
      partial.tile.tileContainer (paths.testruns_link user) "Testruns" counts.TestRuns "red" "toggle-right"
    ]
    row [ m12 [ partial.testruns.testrun testrunRows ] ]
  ]

let html session user counts testruns =
  base_html
    "home"
    [
      views.partial.sidebar.left_sidebar session user counts
      home_content user counts testruns
    ]
    scripts.none
