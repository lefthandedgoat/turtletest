module applications

open Suave.Html
open html_common
open html_bootstrap

let applications_content executionRows =
  mcontent [
    row [
      m12 [
        partial_executions.execution executionRows
      ]
    ]
  ]

let html counts executions =
  let html' =
    html [
      head "applications"
      body [
        wrapper [
          partial_sidebar.left_sidebar counts
          applications_content executions
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
