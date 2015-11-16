module views.testrunsCreate

open Suave.Html
open html_common
open html_bootstrap
open forms.newtypes
open views.partial

let empty = ""

let testrun_details applications =
  block_flat [
    header [ h3 "Create Test Case" ]
    content [
      form_horizontal [
        label_select "Application" (views.suites.applicationsToSelect applications)
        label_text "Description" empty
        label_text "Test Cases" empty
        form_group [ sm10 [ pull_right [ button_save ] ] ]
      ]
    ]
  ]

let testrun_content applications =
  mcontent [ row [ m12 [ testrun_details applications ] ] ]

let html session user counts applications =
  base_html
    "create test run"
    [
      partial.sidebar.left_sidebar session user counts
      testrun_content applications
    ]
    scripts.none

let error_testrun_details applications errors (newTestRun : NewTestRun)=
  block_flat [
    header [ h3 "Create Test Run" ]
    content [
      form_horizontal [
        errored_label_select "Application" (views.suites.applicationsToSelect applications) newTestRun.Application errors
        errored_label_text "Description" newTestRun.Description errors
        errored_label_text "Test Cases" newTestRun.TestCases errors
        form_group [ sm10 [ pull_right [ button_save ] ] ]
      ]
    ]
  ]

let error_testrun_content applications errors newTestrun =
  mcontent [ row [ m12 [ error_testrun_details applications errors newTestrun ] ] ]

let error_html session user counts applications errors newTestrun =
  base_html
    "create test case"
    [
      partial.sidebar.left_sidebar session user counts
      error_testrun_content applications errors newTestrun
    ]
    scripts.none
