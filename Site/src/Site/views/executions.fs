module views.executions

open Suave.Html
open html_common
open html_bootstrap
open types
open views.partial

let html user counts =
  base_html
    "executions"
    [
      partial.sidebar.left_sidebar user counts
      h1 "Coming Soon"
    ]
    scripts.none
