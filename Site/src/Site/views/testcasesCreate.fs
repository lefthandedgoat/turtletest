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

let error_testcase_details applications suites errors (newTestCase : forms.NewTestCase)=
  block_flat [
    header [ h3 "Create Test Case" ]
    content [
      form_horizontal [
        errored_label_select "Application" applications newTestCase.Application errors
        errored_label_select "Suite" suites newTestCase.Suite errors
        errored_label_text "Name" newTestCase.Name errors
        errored_label_text "Version" newTestCase.Version errors
        errored_label_text "Owners" newTestCase.Owners errors
        label_textarea "Notes" newTestCase.Notes
        errored_label_text "Requirements" newTestCase.Requirements errors
        errored_label_text "Steps" newTestCase.Steps errors
        errored_label_text "Expected" newTestCase.Expected errors
        errored_label_text "History" newTestCase.History errors
        errored_label_text "Attachments" newTestCase.Attachments errors
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let error_testcase_content applications suites errors newTestCase =
  mcontent [
    row [
      m12 [
        error_testcase_details applications suites errors newTestCase
      ]
    ]
  ]

let error_html user counts applications suites errors newTestCase =
  base_html
    "create test case"
    [
      partial_sidebar.left_sidebar user counts
      error_testcase_content applications suites errors newTestCase
    ]
    scripts.none
