module views.applications

open Suave.Html
open html_common
open html_bootstrap
open types.read
open types.permissions
open views.partial

let privateOptions = ["True","Yes"; "False","No"]

let application_create_button user =
  button_small_plain (paths.applicationCreate_link user) [ text "Create"]

let application_create_button_success user =
  button_small_success (paths.applicationCreate_link user) [ text "Create"]

let application_edit_button user id =
  button_small_success (paths.applicationEdit_link user id) [ text "Edit"]

let suite_create_button user id =
  button_small_success (paths.suiteCreate_queryStringLink user id) [ text "Create"]

let application_details (application : Application ) buttons =
  block_flat [
    header [ h3Inner application.Name [ buttons ] ]
    content [
      form_horizontal [
        //label_text_ahref_button "Address" application.Address "Go!"
        label_static "Address" application.Address
        //label_text_ahref_button "Documentation" application.Documentation "Go!"
        label_static "Documentation" application.Documentation
        label_static "Owners" application.Owners
        label_static "Developers" application.Developers
        label_static "Notes" application.Notes
        label_static "Private" (string application.Private)
      ]
    ]
  ]

let suites_grid user suites applicationId =
  let toTr (suite : Suite) inner =
    trLink (paths.suite_link user suite.Id) inner

  let toTd (suite : Suite) =
    [
      td [ text (string suite.Name) ]
      td [ text (string suite.Version) ]
      td [ text (string suite.Owners) ]
    ]
  block_flat [
    header [ h3Inner "Suites" [ pull_right [ suite_create_button user (sprintf "applicationId=%i" applicationId) ] ] ]
    content [
      table_bordered_linked_tr
        [
          "Name"
          "Version"
          "Owners"
        ]
        suites toTd toTr
    ]
  ]

let grid user applications buttons =
  let toTr (app : Application) inner =
    trLink (paths.application_link user app.Id) inner

  let toTd (app : Application) =
    [
      td [ text (string app.Name) ]
      td [ text (string app.Owners) ]
      td [ text (string app.Developers) ]
      td [ yesNo app.Private ]
    ]
  block_flat [
    header [ h3Inner "Applications" [ pull_right [ buttons ] ] ]
    content [
      table_bordered_linked_tr
        [
          "Name"
          "Owners"
          "Developers"
          "Private"
        ]
        applications toTd toTr
    ]
  ]

let application_content permission user testrunRows (application : Application) suites =
  let edit_and_create_buttons =
    if ownerOrContributor permission
    then pull_right [ application_create_button user; application_edit_button user application.Id ]
    else emptyText

  mcontent [
    row [ m12 [ application_details application edit_and_create_buttons ] ]
    row [ m12 [ suites_grid user suites application.Id ] ]
    row [ m12 [ partial.testruns.testrun testrunRows ] ]
  ]

let applications_content permission user applications =
  let create_button =
    if ownerOrContributor permission
    then application_create_button_success user
    else emptyText

  mcontent [
    row [ m12 [ grid user applications create_button ] ]
  ]

let details session permission user counts testruns application suites =
  base_html
    "application - details"
    [
      partial.sidebar.left_sidebar session user counts
      application_content permission user testruns application suites
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
