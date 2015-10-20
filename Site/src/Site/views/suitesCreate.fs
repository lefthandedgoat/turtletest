module suitesCreate

open Suave.Html
open html_common
open html_bootstrap
open types

let suite_details =
  block_flat [
    header [ h3 "Create Suite" ]
    content [
      form_horizontal [
        label_text "Application" ""
        label_text "Name" ""
        label_text "Version" ""
        label_text "Owners" ""
        label_textarea "Notes" ""
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let suite_content =
  mcontent [
    row [
      m12 [
        suite_details
      ]
    ]
  ]

let html user counts =
  let html' =
    html [
      base_head "create suite"
      body [
        wrapper [
          partial_sidebar.left_sidebar user counts
          suite_content
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
