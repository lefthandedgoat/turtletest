module views.testcasesCreate

open Suave.Html
open html_common
open html_bootstrap
open forms.newtypes
open views.partial

let empty = ""

let testcase_details applications suites applicationId suiteId =
  block_flat [
    header [ h3 "Create Test Case" ]
    content [
      form_horizontal [
        label_select_selected "Application" (views.suites.applicationsToSelect applications) applicationId
        label_select_selected "Suite" (views.testcases.suitesToSelect suites) suiteId
        label_text "Name" empty
        label_text "Version" empty
        label_text "Owners" empty
        label_textarea "Notes" empty
        label_text "Requirements" empty
        label_text "Steps" empty
        label_text "Expected" empty
        label_text "History" empty
        label_text "Attachments" empty
        form_group [ sm10 [ pull_right [ button_save ] ] ]
      ]
    ]
  ]

let testcase_content applications suites applicationId suiteId =
  mcontent [ row [ m12 [ testcase_details applications suites applicationId suiteId ] ] ]

let html session user counts applications suites applicationId suiteId =
  base_html
    "create test case"
    [
      partial.sidebar.left_sidebar session user counts
      testcase_content applications suites applicationId suiteId
    ]
    scripts.none

let error_testcase_details applications suites errors (newTestCase : NewTestCase)=
  block_flat [
    header [ h3 "Create Test Case" ]
    content [
      form_horizontal [
        errored_label_select "Application" (views.suites.applicationsToSelect applications) newTestCase.Application errors
        errored_label_select "Suite" (views.testcases.suitesToSelect suites) newTestCase.Suite errors
        errored_label_text "Name" newTestCase.Name errors
        errored_label_text "Version" newTestCase.Version errors
        errored_label_text "Owners" newTestCase.Owners errors
        errored_label_textarea "Notes" newTestCase.Notes errors
        errored_label_text "Requirements" newTestCase.Requirements errors
        errored_label_text "Steps" newTestCase.Steps errors
        errored_label_text "Expected" newTestCase.Expected errors
        errored_label_text "History" newTestCase.History errors
        errored_label_text "Attachments" newTestCase.Attachments errors
        form_group [ sm10 [ pull_right [ button_save ] ] ]
      ]
    ]
  ]

let error_testcase_content applications suites errors newTestCase =
  mcontent [ row [ m12 [ error_testcase_details applications suites errors newTestCase ] ] ]

let error_html session user counts applications suites errors newTestCase =
  base_html
    "create test case"
    [
      partial.sidebar.left_sidebar session user counts
      error_testcase_content applications suites errors newTestCase
    ]
    scripts.none
