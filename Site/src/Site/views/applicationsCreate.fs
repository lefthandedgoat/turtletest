module applicationsCreate

open Suave.Html
open html_common
open html_bootstrap
open types

let empty = ""

let applications_details =
  block_flat [
    header [ h3 "Create Application" ]
    content [
      form_horizontal [
        label_text "Name" empty
        label_text "Address" empty
        label_text "Documentation" empty
        label_text "Owners" empty
        label_text "Developers" empty
        label_textarea "Notes" empty
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
  base_html
    "create application"
    [
      partial_sidebar.left_sidebar user counts
      applications_content
    ]
    scripts.none

let error_applications_details errors (newApplication : forms.NewApplication )=
  block_flat [
    header [ h3 "Create Application" ]
    content [
      form_horizontal [
        errored_label_text "Name" newApplication.Name errors
        errored_label_text "Address" newApplication.Address errors
        errored_label_text "Documentation" newApplication.Documentation errors
        errored_label_text "Owners" newApplication.Owners errors
        errored_label_text "Developers" newApplication.Developers errors
        label_textarea "Notes" newApplication.Notes
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let error_applications_content errors newApplication =
  mcontent [
    row [
      m12 [
        error_applications_details errors newApplication
      ]
    ]
  ]

let error_html user counts errors newApplication =
  base_html
    "create application"
    [
      partial_sidebar.left_sidebar user counts
      error_applications_content errors newApplication
    ]
    scripts.none
