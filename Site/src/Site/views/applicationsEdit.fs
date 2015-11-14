module views.applicationsEdit

open Suave.Html
open html_common
open html_bootstrap
open types.read
open forms.edittypes
open views.partial

let empty = ""
let privateOptions = ["True","Yes"; "False","No"]

let applications_details (editApplication : Application) =
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
        label_select_selected "Private" privateOptions (string editApplication.Private)
        form_group [ sm10 [ pull_right [ button_save ] ] ]
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

let html session user counts editApplication =
  base_html
    "edit application"
    [
      partial.sidebar.left_sidebar session user counts
      applications_content editApplication
    ]
    scripts.none

let error_applications_details errors (editApplication : EditApplication) =
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
        errored_label_select "Private" privateOptions editApplication.Private errors
        form_group [ sm10 [ pull_right [ button_save ] ] ]
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

let error_html session user counts errors editApplication =
  base_html
    "edit application"
    [
      partial.sidebar.left_sidebar session user counts
      error_applications_content errors editApplication
    ]
    scripts.none
