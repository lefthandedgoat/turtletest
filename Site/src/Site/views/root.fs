module views.root

open Suave.Html
open html_common
open html_bootstrap
open types.response

let head =
  head [
    meta ["charset","utf-8"]
    meta ["name","viewport"; "content","width=device-width, initial-scale=1.0"]
    meta ["name","description"; "content",""]
    meta ["name","author"; "content",""]
    cssLink "http://fonts.googleapis.com/css?family=Lato:400,700"
    cssLink "http://fonts.googleapis.com/css?family=Pacifico"
    link
    title (sprintf "turtle test - test case management")
    cssLink "/css/bootstrap.min.css"
    cssLink "/css/font-awesome.min.css"
    cssLink "/css/root-theme-light.css"
    cssLink "/css/root-theme-turtle-purple.css"
  ]

let private handleResponse (rootResponse : RootResponse) =
  match rootResponse with
    | Get ->
        sm6 [
          input_group [
            input_form_control_inner "Email Address" "Email" "" [
              input_group_button [
                inputAttr [ "value","I'm Interested!"; "type","submit"; "class","btn btn-primary"; "style","background-color:#7761a7" ]
              ]
            ]
          ]
        ]
    | Success -> sm6 [ h1 "Thanks!" ]

let html (rootResponse : RootResponse) =
  let html' =
    html [
      head
      body [
        container [
          row [
            headerId "logo" [
              m12 [
                imgSrc "/images/turtle-medium.png"
              ]
            ]
            sectionId "marketing-text" [
              m12 [
                h1 "Stay tuned, we are launching very soon..."
                p [ text "turtle test is working hard to launch a new site that's going to greatly improve the way you manage test cases. Leave us your email below, and we'll notify you the minute we open the doors." ]
                form_horizontal [
                  form_group [
                    sm3 [ emptyText ]
                    handleResponse rootResponse
                    sm3 [ emptyText ]
                  ]
                ]
              ]
            ]
            footer [
              m12 [
                pClass "social-text" [
                  span [ emptyText ]
                  text """<i>For more details leading up to our launch <br/>check us out on <a href="https://twitter.com/lefthandedgoat">twitter!</a></i>"""
                ]
                text """<small><i>&copy;2015 turtle test. All rights reserved.</i></small>"""
              ]
            ]
          ]
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
