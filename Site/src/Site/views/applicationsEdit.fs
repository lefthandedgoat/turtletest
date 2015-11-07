module applicationsEdit

open Suave.Html
open html_common
open html_bootstrap
open types

let empty = ""

let applications_details (editApplication : types.Application) =
  block_flat [
    header [ h3 "Edit Application" ]
    content [
      form_horizontal [
        label_text "Name" editApplication.Name
        label_text "Address" editApplication.Address
        label_text "Documentation" editApplication.Documentation
        label_text "Owners" editApplication.Owners
        label_text "Developers" editApplication.Developers
        label_textarea "Notes" editApplication.Notes
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let applications_content editApplication =
  mcontent [
    row [
      m12 [
        applications_details editApplication
      ]
    ]
  ]

let html user counts editApplication =
  base_html
    "edit application"
    [
      partial_sidebar.left_sidebar user counts
      applications_content editApplication
    ]
    scripts.none

let error_applications_details errors (editApplication : forms.EditApplication) =
  block_flat [
    header [ h3 "Edit Application" ]
    content [
      form_horizontal [
        errored_label_text "Name" editApplication.Name errors
        errored_label_text "Address" editApplication.Address errors
        errored_label_text "Documentation" editApplication.Documentation errors
        errored_label_text "Owners" editApplication.Owners errors
        errored_label_text "Developers" editApplication.Developers errors
        errored_label_textarea "Notes" editApplication.Notes errors
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let error_applications_content errors editApplication =
  mcontent [
    row [
      m12 [
        error_applications_details errors editApplication
      ]
    ]
  ]

let error_html user counts errors editApplication =
  base_html
    "edit application"
    [
      partial_sidebar.left_sidebar user counts
      error_applications_content errors editApplication
    ]
    scripts.none