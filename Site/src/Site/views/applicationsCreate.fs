module views.applicationsCreate

open Suave.Html
open html_common
open html_bootstrap
open forms.newtypes
open views.partial

let empty = ""
let privateOptions = ["True","Yes"; "False","No"]

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
        label_select "Private" privateOptions
        form_group [ sm10 [ pull_right [ button_save ] ] ]
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

let html session user counts =
  base_html
    "create application"
    [
      partial.sidebar.left_sidebar session user counts
      applications_content
    ]
    scripts.none

let error_applications_details errors (newApplication : NewApplication) =
  block_flat [
    header [ h3 "Create Application" ]
    content [
      form_horizontal [
        errored_label_text "Name" newApplication.Name errors
        errored_label_text "Address" newApplication.Address errors
        errored_label_text "Documentation" newApplication.Documentation errors
        errored_label_text "Owners" newApplication.Owners errors
        errored_label_text "Developers" newApplication.Developers errors
        errored_label_textarea "Notes" newApplication.Notes errors
        errored_label_select "Private" privateOptions newApplication.Private errors
        form_group [ sm10 [ pull_right [ button_save ] ] ]
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

let error_html session user counts errors newApplication =
  base_html
    "create application"
    [
      partial.sidebar.left_sidebar session user counts
      error_applications_content errors newApplication
    ]
    scripts.none
