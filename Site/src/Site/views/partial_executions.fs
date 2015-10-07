module partial_executions

open Suave.Html
open html_common
open html_bootstrap
open types

let executionRow suite name percent =
  let color =
    if percent >= 75 then "success"
    else if percent >= 50 then "warning"
    else "danger"
  let percent = (string percent) + "%"

  trClass "items" [
    tdAttr [ "style","width: 10%;" ] [ labelX color (text "Execution") ]
    td [ p [ strong suite ] ]
    td [ p [ span [(text name) ] ] ]
    tdColor color [ divClass "progress" [ progress_bar color percent (text percent) ]]
  ]

let execution executionRows =
  block_flat [
    header [ h3 "Recent Executions" ]
    content [
      table_responsive [
        tableClass "no-border hover list" [
          tbodyClass "no-border-y" (executionRows |> List.map (fun er -> executionRow er.Application er.Description er.Percent))
        ]
      ]
    ]
  ]
