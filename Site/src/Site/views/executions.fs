module executions

open Suave.Html
open html_common
open html_bootstrap
open types

let html user counts =
  base_html
    "executions"
    [
      partial_sidebar.left_sidebar user counts
      h1 "Coming Soon"
    ]
    scripts.none
