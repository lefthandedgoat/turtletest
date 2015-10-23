module testcasesCreate

open Suave.Html
open html_common
open html_bootstrap
open types

let testcase_details applications suites =
  block_flat [
    header [ h3 "Create Test Case" ]
    content [
      form_horizontal [
        label_select "Application" applications
        label_select "Suite" suites
        label_text "Name" ""
        label_text "Version" ""
        label_text "Owners" ""
        label_textarea "Notes" ""
        label_text "Requirements" ""
        label_text "Steps" ""
        label_text "Expected" ""
        label_text "History" ""
        label_text "Attachments" ""
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let testcase_content applications suites =
  mcontent [
    row [
      m12 [
        testcase_details applications suites
      ]
    ]
  ]

let html user counts applications suites =
  let html' =
    html [
      base_head "create test case"
      body [
        wrapper [
          partial_sidebar.left_sidebar user counts
          testcase_content applications suites
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
