module suitesCreate

open Suave.Html
open html_common
open html_bootstrap
open types

let suite_details applications =
  block_flat [
    header [ h3 "Create Suite" ]
    content [
      form_horizontal [
        label_select "Application" applications
        label_text "Name" ""
        label_text "Version" ""
        label_text "Owners" ""
        label_textarea "Notes" ""
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let suite_content applications =
  mcontent [
    row [
      m12 [
        suite_details applications
      ]
    ]
  ]

let html user counts applications =
  let html' =
    html [
      base_head "create suite"
      body [
        wrapper [
          partial_sidebar.left_sidebar user counts
          suite_content applications
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
