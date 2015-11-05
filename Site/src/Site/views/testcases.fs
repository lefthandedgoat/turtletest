module testcases

open Suave.Html
open html_common
open html_bootstrap
open types

let testcase_create_button user =
  button_create (paths.testcasesCreate_link user) [ text "Create"]

let testcase_details (testcase : types.TestCase ) =
  block_flat [
    header [ h3 testcase.Name ]
    content [
      form_horizontal [
        label_text "Application" testcase.ApplicationName
        label_text "Suite" testcase.SuiteName
        label_text "Name" testcase.Name
        label_text "Version" testcase.Version
        label_text "Owners" testcase.Owners
        label_textarea "Notes" testcase.Notes
        label_text_ahref_button "Requirements" testcase.Requirements "Go!"
        label_text "Steps" testcase.Steps
        label_text "Expected" testcase.Expected
        label_text "History" testcase.History
        label_text "Attachments" testcase.Attachments
      ]
    ]
  ]

let testcase_content user testcase =
  mcontent [
    row_nomargin [
      m12 [
        testcase_create_button user
      ]
    ]
    row [
      m12 [
        testcase_details testcase
      ]
    ]
  ]

let html user testcase counts =
  base_html
    "test case"
    [
      partial_sidebar.left_sidebar user counts
      testcase_content user testcase
    ]
    scripts.none
