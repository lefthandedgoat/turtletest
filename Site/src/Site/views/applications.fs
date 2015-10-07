module applications

open Suave.Html
open html_common
open html_bootstrap
open types

let applications_details (application : types.Application ) =
  block_flat [
    header [ h3 application.Name ]
    content [
      form_horizontal [
        form_group [
          control_label [ text application.Name ]
          sm8 [
            input_group [
              input_form_control application.Address
              input_group_button [
                button_primary [ text "Go!" ]
              ]
            ]
          ]
        ]
      ]
    ]
  ]

let applications_content executionRows application =
  mcontent [
    row [
      m12 [
        applications_details application
      ]
    ]
    row [
      m12 [
        partial_executions.execution executionRows
      ]
    ]

  ]

let html counts executions application =
  let html' =
    html [
      head "applications"
      body [
        wrapper [
          partial_sidebar.left_sidebar counts
          applications_content executions application
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
