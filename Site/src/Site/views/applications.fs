module applications

open Suave.Html
open html_common
open html_bootstrap
open types

let privateOptions = ["True","Yes"; "False","No"]

let application_create_button user =
  button_create (paths.applicationCreate_link user) [ text "Create"]

let application_edit_button user id =
  button_edit (paths.applicationEdit_link user id) [ text "Edit"]

let application_details (application : types.Application ) =
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

let application_content user executionRows (application : types.Application) suites =
  mcontent [
    row_nomargin [ m12 [ application_edit_button user application.Id; application_create_button user ] ]
    row [ m12 [ application_details application ] ]
    row [ m12 [ suites_grid user suites ] ]
    row [ m12 [ partial_executions.execution executionRows ] ]
  ]

let applications_content user applications =
  mcontent [
    row_nomargin [ m12 [ application_create_button user ] ]
    row [ m12 [ grid user applications ] ]
  ]

let details user counts executions application suites =
  base_html
    "application - details"
    [
      partial_sidebar.left_sidebar user counts
      application_content user executions application suites
    ]
    scripts.applications_bundle

let list user counts (applications : Application list) =
  base_html
    "applications"
    [
      partial_sidebar.left_sidebar user counts
      applications_content user applications
    ]
    scripts.applications_bundle
