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
        label_text_ahref_button "Address" application.Address "Go!"
        label_text_ahref_button "Documentation" application.Documentation "Go!"
        label_text "Owners" application.Owners
        label_text "Developers" application.Developers
        label_textarea "Notes" application.Notes
      ]
    ]
  ]

let applications_grid suites =
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
        suites
    ]
  ]

let applications_content executionRows application suites =
  mcontent [
    row [
      m12 [
        applications_details application
      ]
    ]
    row [
      m12 [
        applications_grid suites
      ]
    ]
    row [
      m12 [
        partial_executions.execution executionRows
      ]
    ]

  ]

let html user counts executions application suites =
  let html' =
    html [
      base_head "applications"
      body [
        wrapper [
          partial_sidebar.left_sidebar user counts
          applications_content executions application suites
        ]
        (text scripts.jquery_1_11_3_min)
        (text scripts.datatable_jquery_1_10_9_min)
        (text scripts.datatable_min)
        (text scripts.datatables_bootstrap_adapter)
        (text scripts.applications_datatable)
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
