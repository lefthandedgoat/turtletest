module html_bootstrap

open System

open Suave
open Suave.Cookie
open Suave.Http
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Http.Applicatives
open Suave.Model.Binding
open Suave.State.CookieStateStore
open Suave.Types
open Suave.Web
open Suave.Html
open html_common

let icon type' = italic ["class", (sprintf "fa fa-%s" type')] emptyText
let wrapper inner = divClass "cl-wrapper" inner
let sidebar inner = divClass "cl-sidebar" inner
let toggle inner = divClass "cl-toggle" inner
let navblock inner = divClass "cl-navblock" inner
let menu_space inner = divClass "menu-space" inner
let content inner = divClass "content" inner
let sidebar_logo inner = divClass "sidebar-logo" inner
let logo inner = divClass "logo" inner
let vnavigation inner = ulAttr ["class", "cl-vnavigation"] inner
//            <li><a href="applications.html"><i class="fa fa-desktop"></i>
//                <span>Applications</span>
//                <span class="badge badge-primary pull-right">4</span>
//            </a></li>

let side_link link text' count style icon' =
  li [
    aHref link [
      icon icon'
      span [text text']
      spanAttr ["class", sprintf "badge badge-%s pull-right" style] [text (string count)]
    ]
  ]
