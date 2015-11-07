module suites

open Suave.Html
open html_common
open html_bootstrap
open types

let suite_create_button user =
  button_create (paths.suiteCreate_link user) [ text "Create"]

let suite_details (suite : types.Suite ) =
  block_flat [
    header [ h3 suite.Name ]
    content [
      form_horizontal [
        label_text "Application" suite.ApplicationId
        label_text "Name" suite.Name
        label_text "Version" suite.Version
        label_text "Owners" suite.Owners
        label_textarea "Notes" suite.Notes
      ]
    ]
  ]

let testcases_grid testcases =
  let toTd row = row |> List.map(fun cell' -> td [text cell'])
  block_flat [
    header [ h3 "Test Cases" ]
    content [
      table_bordered
        [
          "Column 1"
          "Column 2"
          "Column 3"
          "Column 4"
          "Column 5"
        ]
        testcases toTd
    ]
  ]

let grid user suites =
  let toTd (suite : Suite) =
    [
      td [ aHref (paths.suite_link user suite.Id) [ text (string suite.Id) ] ]
      td [ text (string suite.Name) ]
      td [ text (string suite.Version) ]
      td [ text (string suite.Owners) ]
    ]
  block_flat [
    header [ h3 "Suites" ]
    content [
      table_bordered
        [
          "Id"
          "Name"
          "Version"
          "Owners"
        ]
        suites toTd
    ]
  ]

let suite_content user suite testcases =
  mcontent [
    row_nomargin [ m12 [ suite_create_button user ] ]
    row [ m12 [ suite_details suite ] ]
    row [ m12 [ testcases_grid testcases ] ]
  ]

let suites_content user applications =
  mcontent [
    row_nomargin [ m12 [ suite_create_button user ] ]
    row [ m12 [ grid user applications ] ]
  ]

let details user suite testcases counts =
  base_html
    "suites"
    [
      partial_sidebar.left_sidebar user counts
      suite_content user suite testcases
    ]
    scripts.applications_bundle

let list user counts (suites : Suite list) =
  base_html
    "suites"
    [
      partial_sidebar.left_sidebar user counts
      suites_content user suites
    ]
    scripts.applications_bundle
