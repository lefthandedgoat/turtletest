module login

open Suave.Html
open html_common
open html_bootstrap
open types

let empty = ""

let login_details error email=
  let errorTag =
    if error then
      stand_alone_error "Invalid username or password"
    else
      emptyText

  block_flat [
    header [ h3 "Login" ]
    content [
      form_horizontal [
        errorTag
        label_text "Email" email
        label_password "Password" empty
        form_group [ sm10 [ button_submit ] ]
      ]
    ]
  ]

let login_content error email =
  mcontent [
    row [
      m12 [
        login_details error email
      ]
    ]
  ]

let html error email =
  let html' =
    html [
      base_head "login"
      body [
        wrapper [
          login_content error email
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
