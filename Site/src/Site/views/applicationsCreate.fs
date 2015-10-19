module applicationsCreate

open Suave.Html
open html_common
open html_bootstrap
open types

let applications_details =
  block_flat [
    header [ h3 "Create Application" ]
    content [
      form_horizontal [
        label_text "Address" ""
        label_text "Documentation" ""
        label_text "Owners" ""
        label_text "Developers" ""
        label_textarea "Notes" ""
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let applications_content =
  mcontent [
    row [
      m12 [
        applications_details
      ]
    ]
  ]

let html user counts =
  let html' =
    html [
      base_head "applications"
      body [
        wrapper [
          partial_sidebar.left_sidebar user counts
          applications_content
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
