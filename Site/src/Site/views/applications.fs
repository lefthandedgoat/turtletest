module applications

open Suave.Html
open html_common
open html_bootstrap
open types

let applications_details (application : types.Application ) =
  block_flat [
    header [ h3 application.Name ]
    content [
      form_horizontal [
        label_text_button "Address" application.Address "Go!"
        label_text_button "Documentation" application.Documentation "Go!"
        label_text "Owners" application.Owners
        label_text "Developers" application.Developers
        label_textarea "Notes" application.Notes
      ]
    ]
  ]

let applications_content executionRows application =
  mcontent [
    row [
      m12 [
        applications_details application
      ]
    ]
    row [
      m12 [
        partial_executions.execution executionRows
      ]
    ]

  ]

let html counts executions application =
  let html' =
    html [
      head "applications"
      body [
        wrapper [
          partial_sidebar.left_sidebar counts
          applications_content executions application
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
