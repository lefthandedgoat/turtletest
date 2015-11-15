module views.testruns

open Suave.Html
open html_common
open html_bootstrap
open types
open views.partial

let html session user counts =
  base_html
    "test runs"
    [
      partial.sidebar.left_sidebar session user counts
      h1 "Coming Soon"
    ]
    scripts.none
