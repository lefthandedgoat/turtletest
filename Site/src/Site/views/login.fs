module views.login

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
    div [
      form_horizontal [
        content [
          errorTag
          icon_label_text "Email" email "user"
          icon_password_text "Password" empty "lock"
          form_group [ sm12 [ pull_right [ button_register; button_login ] ] ]
        ]
      ]
    ]
  ]

let login_content error email =
  divClass "middle-login" [
    login_details error email
  ]

let html error email =
  login_html
    "login"
    [login_content error email]
    scripts.none
