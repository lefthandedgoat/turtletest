module views.applications

open Suave.Html
open html_common
open html_bootstrap
open types.read
open types.permissions
open views.partial

let privateOptions = ["True","Yes"; "False","No"]

let application_create_button user =
  button_small_create (paths.applicationCreate_link user) [ text "Create"]

let application_edit_button user id =
  button_small_edit (paths.applicationEdit_link user id) [ text "Edit"]

let suite_create_button user =
  button_small_create (paths.suiteCreate_link user) [ text "Create"]

let application_details (application : Application ) buttons =
  block_flat [
    header [ h3Inner application.Name [ buttons ] ]
    content [
      form_horizontal [
        label_text_ahref_button "Address" application.Address "Go!"
        label_text_ahref_button "Documentation" application.Documentation "Go!"
        label_text "Owners" application.Owners
        label_text "Developers" application.Developers
        label_textarea "Notes" application.Notes
        label_select_selected "Private" privateOptions (string application.Private)
      ]
    ]
  ]

let suites_grid user suites =
  let toTd (suite : Suite) =
    [
      td [ aHref (paths.suite_link user suite.Id) [ text (string suite.Id) ] ]
      td [ text (string suite.Name) ]
      td [ text (string suite.Version) ]
      td [ text (string suite.Owners) ]
    ]
  block_flat [
    header [ h3Inner "Suites" [ pull_right [ suite_create_button user ] ] ]
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

let grid user applications buttons =
  let toTd (app : Application) =
    [
      td [ aHref (paths.application_link user app.Id) [ text (string app.Id) ] ]
      td [ text (string app.Name) ]
      td [ text (string app.Owners) ]
      td [ text (string app.Developers) ]
    ]
  block_flat [
    header [ h3Inner "Applications" [ pull_right [ buttons ] ] ]
    content [
      table_bordered
        [
          "Id"
          "Name"
          "Owners"
          "Developers"
        ]
        applications toTd
    ]
  ]

let application_content permission user executionRows (application : Application) suites =
  let edit_and_create_buttons =
    if ownerOrContributor permission
    then pull_right [ application_edit_button user application.Id; application_create_button user ]
    else emptyText

  mcontent [
    row [ m12 [ application_details application edit_and_create_buttons ] ]
    row [ m12 [ suites_grid user suites ] ]
    row [ m12 [ partial.executions.execution executionRows ] ]
  ]

let applications_content permission user applications =
  let create_button =
    if ownerOrContributor permission
    then application_create_button user
    else emptyText

  mcontent [
    row [ m12 [ grid user applications create_button ] ]
  ]

let details session permission user counts executions application suites =
  base_html
    "application - details"
    [
      partial.sidebar.left_sidebar session user counts
      application_content permission user executions application suites
    ]
    scripts.applications_bundle

let list session permission user counts (applications : Application list) =
  base_html
    "applications"
    [
      partial.sidebar.left_sidebar session user counts
      applications_content permission user applications
    ]
    scripts.applications_bundle
