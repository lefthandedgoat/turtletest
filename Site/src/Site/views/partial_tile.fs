module views.partial.tile

open Suave.Html
open html_common
open html_bootstrap

let tileContainer link text' count color icon' =
  m3sm6 [
    tile color [
      content [
        h1Class "text-left" (text (string count))
        p [ (text text') ]
      ]
      divClass "icon" [ icon icon' ]
      aHrefAttr link ["class", "details"] [
        text "Details "
        span [ icon "arrow-circle-right pull-right" ]
      ]
    ]
  ]
