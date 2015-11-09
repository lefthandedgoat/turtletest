module views.testcaseEdit

open Suave.Html
open html_common
open html_bootstrap
open types.read
open forms.newtypes
open forms.edittypes
open views.partial

let empty = ""

let testcase_details applications suites (editTestCase : TestCase) =
  block_flat [
    header [ h3 "Edit Test Case" ]
    content [
      form_horizontal [
        label_select_selected "Application" (views.suites.applicationsToSelect applications) (string editTestCase.ApplicationId)
        label_select_selected "Suite" (views.testcases.suitesToSelect suites) (string editTestCase.SuiteId)
        label_text "Name" editTestCase.Name
        label_text "Version" editTestCase.Version
        label_text "Owners" editTestCase.Owners
        label_textarea "Notes" editTestCase.Notes
        label_text "Requirements" editTestCase.Requirements
        label_text "Steps" editTestCase.Steps
        label_text "Expected" editTestCase.Expected
        label_text "History" editTestCase.History
        label_text "Attachments" editTestCase.Attachments
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let testcase_content applications suites editTestCase =
  mcontent [
    row [
      m12 [
        testcase_details applications suites editTestCase
      ]
    ]
  ]

let html session user counts applications suites editTestCase =
  base_html
    "edit test case"
    [
      partial.sidebar.left_sidebar session user counts
      testcase_content applications suites editTestCase
    ]
    scripts.none

let error_testcase_details errors applications suites (editTestCase : EditTestCase) =
  block_flat [
    header [ h3 "Edit Test Case" ]
    content [
      form_horizontal [
        errored_label_select "Application" (views.suites.applicationsToSelect applications) editTestCase.Application errors
        errored_label_select "Suite" (views.testcases.suitesToSelect suites) editTestCase.Suite errors
        errored_label_text "Name" editTestCase.Name errors
        errored_label_text "Version" editTestCase.Version errors
        errored_label_text "Owners" editTestCase.Owners errors
        errored_label_textarea "Notes" editTestCase.Notes errors
        errored_label_text "Requirements" editTestCase.Requirements errors
        errored_label_text "Steps" editTestCase.Steps errors
        errored_label_text "Expected" editTestCase.Expected errors
        errored_label_text "History" editTestCase.History errors
        errored_label_text "Attachments" editTestCase.Attachments errors
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let error_testcase_content errors applications suites editSuite =
  mcontent [
    row [
      m12 [
        error_testcase_details errors applications suites editSuite
      ]
    ]
  ]

let error_html session user counts errors applications suites editTestCase =
  base_html
    "edit test case"
    [
      partial.sidebar.left_sidebar session user counts
      error_testcase_content errors applications suites editTestCase
    ]
    scripts.none
