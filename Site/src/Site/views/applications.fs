module applications

open Suave.Html
open html_common
open html_bootstrap
open types

let application_create_button user =
  button_create (paths.applicationCreate_link user) [ text "Create"]

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
      ]
    ]
  ]

let suites_grid suites =
  let toTd row = row |> List.map(fun cell' -> td [text cell'])
  block_flat [
    header [ h3 "Suites" ]
    content [
      table_bordered
        [
          "Rendering engine"
          "Browser"
          "Platform(s)"
          "Engine version"
          "CSS grade"
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

let application_content user executionRows application suites =
  mcontent [
    row_nomargin [ m12 [ application_create_button user ] ]
    row [ m12 [ application_details application ] ]
    row [ m12 [ suites_grid suites ] ]
    row [ m12 [ partial_executions.execution executionRows ] ]
  ]

let applications_content user applications =
  mcontent [
    row_nomargin [ m12 [ application_create_button user ] ]
    row [ m12 [ grid user applications ] ]
  ]

let details user counts executions application suites =
  let html' =
    html [
      base_head "application - details"
      body [
        wrapper [
          partial_sidebar.left_sidebar user counts
          application_content user executions application suites
        ]
        scripts.applications_bundle |> List.map (fun script -> text script) |> flatten
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'

let list user counts (applications : Application list) =
  let html' =
    html [
      base_head "applications"
      body [
        wrapper [
          partial_sidebar.left_sidebar user counts
          applications_content user applications
        ]
        scripts.applications_bundle |> List.map (fun script -> text script) |> flatten
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
