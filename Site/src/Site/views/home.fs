module home

open Suave.Html
open html_common
open html_bootstrap
open partial_tile
open partial_executions
open types

let home_content counts executionRows =
  mcontent [
    row [
      tileContainer paths.applications "Applications" counts.Applications "purple" "desktop"
      tileContainer paths.suites "Suites" counts.Suites "green" "sitemap"
      tileContainer paths.testcases "Test Cases" counts.TestCases "prusia" "thumbs-up"
      tileContainer paths.executions "Executions" counts.Executions "red" "toggle-right"
    ]
    row [
      m12 [
        execution executionRows
      ]
    ]
  ]

let html counts executions =
  let html' =
    html [
      head "home"
      body [
        wrapper [
          partial_sidebar.left_sidebar counts
          home_content counts executions
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
