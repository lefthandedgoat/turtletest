module suiteEdit

open Suave.Html
open html_common
open html_bootstrap
open types

let empty = ""

let suite_details applications (editSuite : types.Suite) =

  block_flat [
    header [ h3 "Edit Suite" ]
    content [
      form_horizontal [
        label_select_selected "Application" (suites.applicationsToSelect applications) (string editSuite.ApplicationId)
        label_text "Name" editSuite.Name
        label_text "Version" editSuite.Version
        label_text "Owners" editSuite.Owners
        label_textarea "Notes" editSuite.Notes
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let suite_content applications editSuite =
  mcontent [
    row [
      m12 [
        suite_details applications editSuite
      ]
    ]
  ]

let html user counts applications editSuite =
  base_html
    "edit suite"
    [
      partial_sidebar.left_sidebar user counts
      suite_content applications editSuite
    ]
    scripts.none

let error_suite_details errors applications (editSuite : forms.EditSuite) =
  block_flat [
    header [ h3 "Edit Suite" ]
    content [
      form_horizontal [
        errored_label_select "Application" (suites.applicationsToSelect applications) editSuite.Application errors
        errored_label_text "Name" editSuite.Name errors
        errored_label_text "Version" editSuite.Version errors
        errored_label_text "Owners" editSuite.Owners errors
        errored_label_textarea "Notes" editSuite.Notes errors
        form_group [ sm10 [ button_save ] ]
      ]
    ]
  ]

let error_suite_content errors applications editSuite =
  mcontent [
    row [
      m12 [
        error_suite_details errors applications editSuite
      ]
    ]
  ]

let error_html user counts errors applications editSuite =
  base_html
    "edit suite"
    [
      partial_sidebar.left_sidebar user counts
      error_suite_content errors applications editSuite
    ]
    scripts.none
