module suites

open Suave.Html
open html_common
open html_bootstrap
open types

let suite_create_button user =
  button_create (paths.suitesCreate_link user) [ text "Create"]

let suite_details (suite : types.Suite ) =
  block_flat [
    header [ h3 suite.Name ]
    content [
      form_horizontal [
        label_text "Application" suite.ApplicationName
        label_text "Name" suite.Name
        label_text "Version" suite.Version
        label_text "Owners" suite.Owners
        label_textarea "Notes" suite.Notes
      ]
    ]
  ]

let grid testcases =
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

let suites_content user suite testcases =
  mcontent [
    row_nomargin [ m12 [ suite_create_button user ] ]
    row [ m12 [ suite_details suite ] ]
    row [ m12 [ grid testcases ] ]
  ]

let html user suite testcases counts =
  let html' =
    html [
      base_head "suites"
      body [
        wrapper [
          partial_sidebar.left_sidebar user counts
          suites_content user suite testcases
        ]
        scripts.applications_bundle |> List.map (fun script -> text script) |> flatten
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
