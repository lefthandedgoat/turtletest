module views.suites

open Suave.Html
open html_common
open html_bootstrap
open types.read
open views.partial

let suite_create_button user =
  button_small_create (paths.suiteCreate_link user) [ text "Create"]

let suite_edit_button user id =
  button_small_edit (paths.suiteEdit_link user id) [ text "Edit"]

let testcase_create_button user =
  button_small_create (paths.testcaseCreate_link user) [ text "Create"]

let applicationsToSelect applications =
  applications
  |> List.map (fun (app : Application) -> string app.Id, app.Name)
  |> List.sortBy (fun (_, name) -> name)

let suite_details (suite : Suite) applications buttons =
  block_flat [
    header [ h3Inner suite.Name [ pull_right buttons ] ]
    content [
      form_horizontal [
        label_select_selected "Application" (applicationsToSelect applications) (string suite.ApplicationId)
        label_text "Name" suite.Name
        label_text "Version" suite.Version
        label_text "Owners" suite.Owners
        label_textarea "Notes" suite.Notes
      ]
    ]
  ]

let testcases_grid user testcases buttons =
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

let grid user suites buttons =
  let toTd (suite : Suite) =
    [
      td [ aHref (paths.suite_link user suite.Id) [ text (string suite.Id) ] ]
      td [ text (string suite.Name) ]
      td [ text (string suite.Version) ]
      td [ text (string suite.Owners) ]
    ]
  block_flat [
    header [ h3Inner "Suites" [ pull_right buttons ] ]
    content [
      table_bordered
        [
          "Id"
          "Name"
          "Version"
          "Owners"
        ]
        suites toTd
    ]
  ]

let suite_content user (suite : Suite) testcases applications =
  mcontent [
    row [ m12 [ suite_details suite applications [ suite_edit_button user suite.Id; suite_create_button user ] ] ]
    row [ m12 [ testcases_grid user testcases [ testcase_create_button user ] ] ]
  ]

let suites_content user applications =
  mcontent [
    row [ m12 [ grid user applications [ suite_create_button user ] ] ]
  ]

let details session user suite testcases applications counts =
  base_html
    "suite - details"
    [
      partial.sidebar.left_sidebar session user counts
      suite_content user suite testcases applications
    ]
    scripts.applications_bundle

let list session user counts (suites : Suite list) =
  base_html
    "suites"
    [
      partial.sidebar.left_sidebar session user counts
      suites_content user suites
    ]
    scripts.applications_bundle
