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
let block_flat inner = divClass "block-flat" inner
let header inner = divClass "header" inner
let labelX x inner = spanAttr ["class", (sprintf "label label-%s" x)] [inner]
let tdColor color inner = tdAttr ["class", (sprintf "color-%s" color)] inner
let table_responsive inner = divClass "table-responsive" inner
let progress_bar type' percent inner = divAttr ["class", (sprintf "progress-bar progress-bar-%s") type'; "style",(sprintf "width: %s" percent) ] [inner]
let tile color inner = divClass (sprintf "fd-tile detail clean tile-%s" color) inner
let sidebar_item inner = spanAttr ["class","sidebar-item"] inner
