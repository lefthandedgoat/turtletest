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
        label_text "Name" None
        label_text "Version" None
        label_text "Owners" None
        label_textarea "Notes" None
        label_text "Requirements" None
        label_text "Steps" None
        label_text "Expected" None
        label_text "History" None
        label_text "Attachments" None
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
