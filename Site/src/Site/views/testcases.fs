module views.testcases

open Suave.Html
open html_common
open html_bootstrap
open types.read
open types.permissions
open views.partial

let testcase_create_button user =
  button_small_plain (paths.testcaseCreate_link user) [ text "Create"]

let testcase_create_button_success user =
  button_small_success (paths.testcaseCreate_link user) [ text "Create"]

let testcase_edit_button user id =
  button_small_success (paths.testcaseEdit_link user id ) [ text "Edit"]

let suitesToSelect suites =
  suites
  |> List.map (fun (suite : Suite) -> string suite.Id, suite.Name)
  |> List.sortBy (fun (_, name) -> name)

let testcase_details applications suites (testcase : TestCase ) buttons =
  block_flat [
    header [ h3Inner testcase.Name [ pull_right buttons ] ]
    content [
      form_horizontal [
        label_select_selected "Application" (views.suites.applicationsToSelect applications) (string testcase.ApplicationId)
        label_select_selected "Suite" (suitesToSelect suites) (string testcase.SuiteId)
        label_text "Name" testcase.Name
        label_text "Version" testcase.Version
        label_text "Owners" testcase.Owners
        label_textarea "Notes" testcase.Notes
        label_text_ahref_button "Requirements" testcase.Requirements "Go!"
        label_text "Steps" testcase.Steps
        label_text "Expected" testcase.Expected
        label_text "History" testcase.History
        label_text "Attachments" testcase.Attachments
      ]
    ]
  ]

let grid user testcases buttons =
  let toTd (testcase : TestCase) =
    [
      td [ aHref (paths.testcase_link user testcase.Id) [ text (string testcase.Id) ] ]
      td [ text (string testcase.Name) ]
      td [ text (string testcase.Version) ]
      td [ text (string testcase.Owners) ]
    ]
  block_flat [
    header [ h3Inner "Test Cases" [ pull_right buttons ] ]
    content [
      table_bordered
        [
          "Id"
          "Name"
          "Version"
          "Owners"
        ]
        testcases toTd
    ]
  ]

let testcase_content permission user applications suites (testcase : TestCase) =
  let edit_and_create_buttons =
    if ownerOrContributor permission
    then [ testcase_create_button user; testcase_edit_button user testcase.Id ]
    else [ emptyText ]

  mcontent [
    row [ m12 [ testcase_details applications suites testcase edit_and_create_buttons ] ]
  ]

let testcases_content permission user testcases =
  let create_button =
    if ownerOrContributor permission
    then [ testcase_create_button_success user ]
    else [ emptyText ]

  mcontent [
    row [ m12 [ grid user testcases create_button ] ]
  ]

let details session permission user applications suites counts testcase =
  base_html
    "test case - details"
    [
      partial.sidebar.left_sidebar session user counts
      testcase_content permission user applications suites testcase
    ]
    scripts.none

let list session permission user counts (testcases : TestCase list) =
  base_html
    "test cases"
    [
      partial.sidebar.left_sidebar session user counts
      testcases_content permission user testcases
    ]
    scripts.applications_bundle
