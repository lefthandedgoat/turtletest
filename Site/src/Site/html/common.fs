module html_common

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

//emptyText is a work around for Suave.Experimental thinking
//that something is a leaf node and rendering wrong
let emptyText = text ""
let divId id = divAttr ["id", id]
let divClass c = divAttr ["class", c]
let h1 xml = tag "h1" [] xml
let h1Class class' xml = tag "h1" ["class",class'] xml
let h2 s = tag "h2" [] (text s)
let h3 s = tag "h3" [] (text s)
let aHref href inner = tag "a" ["href", href] (flatten inner)
let aHrefAttr href attr inner = tag "a" (("href", href) :: attr) (flatten inner)
let cssLink href = linkAttr [ "href", href; " rel", "stylesheet"; " type", "text/css" ]
let ul xml = tag "ul" [] (flatten xml)
let ulAttr attr xml = tag "ul" attr (flatten xml)
let li inner = tag "li" [] (flatten inner)
let imgSrc src = imgAttr [ "src", src ]
let em s = tag "em" [] (text s)
let strong s = tag "strong" [] (text s)
let meta attr = tag "meta" attr empty
let spanAttr attr inner = tag "span" attr (flatten inner)
let span inner = spanAttr [] inner
let italic attr inner = tag "i" attr inner
let p inner = tag "p" [] (flatten inner)

let form x = tag "form" ["method", "POST"] (flatten x)
let fieldset x = tag "fieldset" [] (flatten x)
let legend txt = tag "legend" [] (text txt)
let submitInput value = inputAttr ["type", "submit"; "value", value]

let tableClass class' x = tag "table" ["class", class'] (flatten x)
let tbodyClass class' x = tag "tbody" ["class", class'] (flatten x)
let th x = tag "th" [] (flatten x)
let tr x = tag "tr" [] (flatten x)
let trClass class' x = tag "tr" ["class", class'] (flatten x)
let td x = tag "td" [] (flatten x)
let tdAttr attr x = tag "td" attr (flatten x)

let head =
  head [
    meta ["charset","utf-8"]
    meta ["name","viewport"; "content","width=device-width, initial-scale=1.0"]
    meta ["name","description"; "content",""]
    meta ["name","author"; "content",""]
    cssLink "http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,400italic,700,800"
    cssLink "http://fonts.googleapis.com/css?family=Raleway:300,200,100"
    link
    title "turtle test"
    cssLink "css/bootstrap.min.css"
    cssLink "css/style.css"
    cssLink "css/font-awesome.min.css"
  ]
