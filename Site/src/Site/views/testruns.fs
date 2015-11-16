module views.testruns

open Suave.Html
open html_common
open html_bootstrap
open types.read
open types.permissions
open views.partial

let testrun_create_button user =
  button_small_plain (paths.testrunCreate_link user) [ text "Create"]

let testrun_create_button_success user =
  button_small_success (paths.testrunCreate_link user) [ text "Create"]

let testrun_edit_button user id =
  button_small_success (paths.testrunEdit_link user id ) [ text "Edit"]

let testrun_details applications (testrun : TestRun ) buttons =
  block_flat [
    header [ h3Inner testrun.Description [ pull_right buttons ] ]
    content [
      form_horizontal [
        label_select_selected "Application" (views.suites.applicationsToSelect applications) (string testrun.ApplicationId)
        label_text "Description" testrun.Description
        label_text "Percent Run" testrun.PercentRun
        label_text "Passed" (sprintf "%A" testrun.Passed)
        label_text "Failed" (sprintf "%A" testrun.Failed)
        label_text "Not Run" (sprintf "%A" testrun.NotRun)
      ]
    ]
  ]

let grid user testcases buttons =
  let toTd (testrun : TestRun) =
    [
      td [ aHref (paths.testrun_link user testrun.Id) [ text (string testrun.Id) ] ]
      td [ text (string testrun.Description) ]
      td [ text (string testrun.RunDate) ]
      td [ text (string testrun.PercentRun) ]
    ]
  block_flat [
    header [ h3Inner "Test Runs" [ pull_right buttons ] ]
    content [
      table_bordered
        [
          "Id"
          "Description"
          "Run Date"
          "Percent Run"
        ]
        testcases toTd
    ]
  ]

let testrun_content permission user applications (testrun : TestRun) =
  let edit_and_create_buttons =
    if ownerOrContributor permission
    then [ testrun_create_button user; testrun_edit_button user testrun.Id ]
    else [ emptyText ]

  mcontent [
    row [ m12 [ testrun_details applications testrun edit_and_create_buttons ] ]
  ]

let testruns_content permission user testruns =
  let create_button =
    if ownerOrContributor permission
    then [ testrun_create_button_success user ]
    else [ emptyText ]

  mcontent [
    row [ m12 [ grid user testruns create_button ] ]
  ]

let details session permission user applications counts testrun =
  base_html
    "test run - details"
    [
      partial.sidebar.left_sidebar session user counts
      testrun_content permission user applications testrun
    ]
    scripts.none

let list session permission user counts (testruns : TestRun list) =
  base_html
    "test runs"
    [
      partial.sidebar.left_sidebar session user counts
      testruns_content permission user testruns
    ]
    scripts.applications_bundle
