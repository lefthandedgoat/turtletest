module views.partial.testruns

open Suave.Html
open html_common
open html_bootstrap
open types.read

let testrunRow suite name percent =
  let color =
    if percent >= 75 then "success"
    else if percent >= 50 then "warning"
    else "danger"
  let percent = (string percent) + "%"

  trClass "items" [
    tdAttr [ "style","width: 10%;" ] [ labelX color (text "Test Run") ]
    td [ p [ strong suite ] ]
    td [ p [ span [(text name) ] ] ]
    tdColor color [ divClass "progress" [ progress_bar color percent (text percent) ]]
  ]

let testrun testrunRows =
  block_flat [
    header [ h3 "Recent Test Runs" ]
    content [
      table_responsive [
        tableClass "no-border hover list" [
          tbodyClass "no-border-y" (testrunRows |> List.map (fun er -> testrunRow er.Application er.Description er.Percent))
        ]
      ]
    ]
  ]
