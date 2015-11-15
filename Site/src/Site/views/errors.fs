module views.errors

open Suave.Html
open html_common
open html_bootstrap

let error_404 =
  error_html
    "Page not found"
    [
      page_error [
        h1Class "number text-center" (text "404")
        h2Class "description text-center" (text "Sorry, but this page does not exist!")
        h3ClassInner "text-center" "Would you like to go " [ aHref "/" [ text "home?" ] ]
        divClass "text-center copy" [ text "&copy; 2015 ";  aHref "/" [ text "turtletest" ] ]
      ]
    ]
    scripts.none

let error_500 =
  error_html
    "Oops!"
    [
      page_error [
        h1Class "number text-center" (text "500")
        h2Class "description text-center" (text "There was a small problem =(.")
        h3Class "text-center" (text "We're trying to fix it, please try again later.")
        h3ClassInner "text-center" "Would you like to go " [ aHref "/" [ text "home?" ] ]
        divClass "text-center copy" [ text "&copy; 2015 ";  aHref "/" [ text "turtletest" ] ]
      ]
    ]
    scripts.none
