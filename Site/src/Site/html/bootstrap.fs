module html_bootstrap

open System

open Suave.Html
open html_common
open types

let icon type' = italic ["class", (sprintf "fa fa-%s" type')] emptyText
let wrapper inner = divId "cl-wrapper" inner
let sidebar inner = divClass "cl-sidebar" inner
let toggle inner = divClass "cl-toggle" inner
let navblock inner = divClass "cl-navblock" inner
let menu_space inner = divClass "menu-space" inner
let content inner = divClass "content" inner
let mcontent inner = divClass "cl-mcont" inner
let sidebar_logo inner = divClass "sidebar-logo" inner
let logo inner = divClass "logo" inner
let vnavigation inner = ulAttr ["class", "cl-vnavigation"] inner
let row inner = divClass "row" inner
let m3sm6 inner = divClass "col-md-3 col-sm-6" inner
let m12 inner = divClass "col-md-12" inner
let sm8 inner = divClass "col-sm-8" inner
let block_flat inner = divClass "block-flat" inner
let header inner = divClass "header" inner
let labelX x inner = spanAttr ["class", (sprintf "label label-%s" x)] [inner]
let tdColor color inner = tdAttr ["class", (sprintf "color-%s" color)] inner
let table_responsive inner = divClass "table-responsive" inner
let progress_bar type' percent inner = divAttr ["class", (sprintf "progress-bar progress-bar-%s") type'; "style",(sprintf "width: %s" percent) ] [inner]
let tile color inner = divClass (sprintf "fd-tile detail clean tile-%s" color) inner
let sidebar_item inner = spanAttr ["class","sidebar-item"] inner
let form_horizontal inner = formAttr ["class","form-horizontal"] inner
let form_group inner = divClass "form-group" inner
let input_group inner = divClass "input-group" inner
let input_form_control placeholder value = inputClassPlaceholder "form-control" placeholder value
let textarea_form_control placeholder value = textareaClassPlaceholder "form-control" placeholder value
let input_group_button inner = spanClass "input-group-btn" inner
let control_label inner = labelClass "col-sm-2 control-label" inner
let button_primary inner = buttonClass "btn btn-primary" inner

let label_text_button label' text' button' =
  form_group [
    control_label [ text label' ]
    sm8 [
      input_group [
        input_form_control label' text'
        input_group_button [
          button_primary [ text button' ]
        ]
      ]
    ]
  ]

let label_text label' text' =
  form_group [
    control_label [ text label' ]
    sm8 [
      input_form_control label' text'
    ]
  ]

let label_textarea label' text' =
  form_group [
    control_label [ text label' ]
    sm8 [
      textarea_form_control label' text'
    ]
  ]

let table_bordered ths rows =
  let table_bordered inner = tableClass "table table-bordered" inner
  table_responsive [
    table_bordered [
      thead [
        tr (ths |> List.map (fun th' -> th [text th']))
      ]
      tbody (rows |> List.map (fun row' -> tr (row' |> List.map(fun cell' -> td [text cell']))))
    ]
  ]
