module views.applications

open Suave.Html
open html_common
open html_bootstrap
open types.read
open types.permissions
open views.partial

let privateOptions = ["True","Yes"; "False","No"]

let application_create_button user =
  button_create (paths.applicationCreate_link user) [ text "Create"]

let application_edit_button user id =
  button_edit (paths.applicationEdit_link user id) [ text "Edit"]

let application_details (application : Application ) =
  block_flat [
    header [ h3 application.Name ]
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
    header [ h3 "Suites" ]
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

let grid user applications =
  let toTd (app : Application) =
    [
      td [ aHref (paths.application_link user app.Id) [ text (string app.Id) ] ]
      td [ text (string app.Name) ]
      td [ text (string app.Owners) ]
      td [ text (string app.Developers) ]
    ]
  block_flat [
    header [ h3 "Applications" ]
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
    then row_nomargin [ m12 [ application_edit_button user application.Id; application_create_button user ] ]
    else emptyText

  mcontent [
    edit_and_create_buttons
    row [ m12 [ application_details application ] ]
    row [ m12 [ suites_grid user suites ] ]
    row [ m12 [ partial.executions.execution executionRows ] ]
  ]

let applications_content permission user applications =
  let create_button =
    if ownerOrContributor permission
    then row_nomargin [ m12 [ application_create_button user ] ]
    else emptyText

  mcontent [
    create_button
    row [ m12 [ grid user applications ] ]
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
