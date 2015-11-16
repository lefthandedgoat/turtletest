module views.testrunEdit

open Suave.Html
open html_common
open html_bootstrap
open types.read
open forms.newtypes
open forms.edittypes
open views.partial

let empty = ""

let testrun_details applications (editTestRun : TestRun) =
  block_flat [
    header [ h3 "Edit Test Run" ]
    content [
      form_horizontal [
        label_select_selected "Application" (views.suites.applicationsToSelect applications) (string editTestRun.ApplicationId)
        label_text "Description" editTestRun.Description
        form_group [ sm10 [ pull_right [ button_save ] ] ]
      ]
    ]
  ]

let testrun_content applications editTestRun =
  mcontent [
    row [
      m12 [
        testrun_details applications editTestRun
      ]
    ]
  ]

let html session user counts applications editTestRun =
  base_html
    "edit test run"
    [
      partial.sidebar.left_sidebar session user counts
      testrun_content applications editTestRun
    ]
    scripts.none

let error_testrun_details errors applications (editTestRun : EditTestRun) =
  block_flat [
    header [ h3 "Edit Test Run" ]
    content [
      form_horizontal [
        errored_label_select "Application" (views.suites.applicationsToSelect applications) editTestRun.Application errors
        errored_label_text "Description" editTestRun.Description errors
        form_group [ sm10 [ pull_right [ button_save ] ] ]
      ]
    ]
  ]

let error_testrun_content errors applications editSuite =
  mcontent [
    row [
      m12 [
        error_testrun_details errors applications editSuite
      ]
    ]
  ]

let error_html session user counts errors applications editTestRun =
  base_html
    "edit test run"
    [
      partial.sidebar.left_sidebar session user counts
      error_testrun_content errors applications editTestRun
    ]
    scripts.none
