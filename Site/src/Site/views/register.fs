module views.register

open Suave.Html
open html_common
open html_bootstrap
open forms.newtypes

let empty = ""

let user_details =
  block_flat [
    header [ h3 "Register" ]
    div [
      form_horizontal [
        content [
          icon_label_text "Name" empty "user"
          icon_label_text "Email" empty "user"
          icon_password_text "Password" empty "lock"
          icon_password_text "Repeat Password" empty "lock"
          form_group [ sm12 [ pull_right [ button_submit ] ] ]
        ]
      ]
    ]
  ]

let register_content =
  divClass "middle-sign-up" [
    user_details
  ]

let html =
  register_html
    "register"
    [register_content]
    scripts.none

let errored_user_details errors (newUser : NewUser)=
  block_flat [
    header [ h3 "Register" ]
    content [
      form_horizontal [
        errored_icon_label_text "Name" newUser.Name "user" errors
        errored_icon_label_text "Email" newUser.Email "user" errors
        errored_icon_password_text "Password" newUser.Password "lock" errors
        errored_icon_password_text "Repeat Password" newUser.RepeatPassword "lock" errors

        //errored_label_text "Name" newUser.Name errors
        //errored_label_text "Email" newUser.Email errors
        //errored_label_password "Password" newUser.Password errors
        //errored_label_password "Repeat Password" newUser.RepeatPassword errors
        form_group [ sm10 [ pull_right [ button_submit ] ] ]
      ]
    ]
  ]

let error_register_content errors newUser =
  divClass "middle-sign-up" [
    errored_user_details errors newUser
  ]

let error_html errors newUser =
  register_html
    "register"
    [error_register_content errors newUser]
    scripts.none
