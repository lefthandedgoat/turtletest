module suites

open Suave.Html
open html_common
open html_bootstrap
open types

let html user counts =
  let html' =
    html [
      base_head "suites"
      body [
        wrapper [
          partial_sidebar.left_sidebar user counts
          h1 "Coming Soon"
        ]
      ]
    ]
    |> xmlToString
  sprintf "<!DOCTYPE html>%s" html'
