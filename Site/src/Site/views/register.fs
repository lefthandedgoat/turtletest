module views.register

open Suave.Html
open html_common
open html_bootstrap
open forms.newtypes

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
  base_html
    "register"
    [register_content]
    scripts.none

let errored_user_details errors (newUser : NewUser)=
  block_flat [
    header [ h3 "Register" ]
    content [
      form_horizontal [
        errored_label_text "Name" newUser.Name errors
        errored_label_text "Email" newUser.Email errors
        errored_label_password "Password" newUser.Password errors
        errored_label_password "Repeat Password" newUser.RepeatPassword errors
        form_group [ sm10 [ button_submit ] ]
      ]
    ]
  ]

let error_register_content errors newUser =
  mcontent [
    row [
      m12 [
        errored_user_details errors newUser
      ]
    ]
  ]

let error_html errors newUser =
  base_html
    "register"
    [error_register_content errors newUser]
    scripts.none
