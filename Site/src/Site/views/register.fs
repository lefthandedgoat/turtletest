module register

open Suave.Html
open html_common
open html_bootstrap
open types

let empty = ""

let user_details =
  block_flat [
    header [ h3 "Register" ]
    content [
      form_horizontal [
        label_text "Name" empty
        label_text "Email" empty
        label_password "Password" empty
        label_password "Repeat Password" empty
        form_group [ sm10 [ button_submit ] ]
      ]
    ]
  ]

let register_content =
  mcontent [
    row [
      m12 [
        user_details
      ]
    ]
  ]

let html =
  let html' =
    html [
      base_head "register"
      body [
        wrapper [
          register_content
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
