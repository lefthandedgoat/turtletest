module testcasesCreate

open Suave.Html
open html_common
open html_bootstrap
open types

let empty = ""

let testcase_details applications suites =
  block_flat [
    header [ h3 "Create Test Case" ]
    content [
      form_horizontal [
        label_select "Application" applications
        label_select "Suite" suites
        label_text "Name" empty
        label_text "Version" empty
        label_text "Owners" empty
        label_textarea "Notes" empty
        label_text "Requirements" empty
        label_text "Steps" empty
        label_text "Expected" empty
        label_text "History" empty
        label_text "Attachments" empty
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
  base_html
    "create test case"
    [
      partial_sidebar.left_sidebar user counts
      testcase_content applications suites
    ]
    scripts.none
