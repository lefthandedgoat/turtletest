module views.suitesCreate

open Suave.Html
open html_common
open html_bootstrap
open forms.newtypes
open views.partial

let empty = ""

let suite_details applications =
  block_flat [
    header [ h3 "Create Suite" ]
    content [
      form_horizontal [
        label_select "Application" (suites.applicationsToSelect applications)
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

let html session user counts applications =
  base_html
    "create suite"
    [
      partial.sidebar.left_sidebar session user counts
      suite_content applications
    ]
    scripts.none

let error_suite_details applications errors (newSuite : NewSuite) =
  block_flat [
    header [ h3 "Create Suite" ]
    content [
      form_horizontal [
        errored_label_select "Application" (suites.applicationsToSelect applications) newSuite.Application errors
        errored_label_text "Name" newSuite.Name errors
        errored_label_text "Version" newSuite.Version errors
        errored_label_text "Owners" newSuite.Owners errors
        errored_label_textarea "Notes" newSuite.Notes errors
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let error_suite_content applications errors newSuite =
  mcontent [
    row [
      m12 [
        error_suite_details applications errors newSuite
      ]
    ]
  ]

let error_html session user counts applications errors newSuite =
  base_html
    "create suite"
    [
      partial.sidebar.left_sidebar session user counts
      error_suite_content applications errors newSuite
    ]
    scripts.none
