module suitesCreate

open Suave.Html
open html_common
open html_bootstrap
open types

let empty = ""

let suite_details applications =
  block_flat [
    header [ h3 "Create Suite" ]
    content [
      form_horizontal [
        label_select "Application" applications
        label_text "Name" empty
        label_text "Version" empty
        label_text "Owners" empty
        label_textarea "Notes" empty
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
  base_html
    "create suite"
    [
      partial_sidebar.left_sidebar user counts
      suite_content applications
    ]
    scripts.none
