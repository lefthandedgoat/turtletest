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
let h1Class class' xml = tag "h1" ["class",class'] xml
let h1 s = tag "h1" [] (text s)
let h2 s = tag "h2" [] (text s)
let h3 s = tag "h3" [] (text s)
let h4 s = tag "h4" [] (text s)
let h5 s = tag "h5" [] (text s)
let h6 s = tag "h6" [] (text s)
let aHref href inner = tag "a" ["href", href] (flatten inner)
let aHrefAttr href attr inner = tag "a" (("href", href) :: attr) (flatten inner)
let cssLink href = linkAttr [ "href", href; " rel", "stylesheet"; " type", "text/css";  ]
let ul xml = tag "ul" [] (flatten xml)
let ulAttr attr xml = tag "ul" attr (flatten xml)
let li inner = tag "li" [] (flatten inner)
let imgSrc src = imgAttr [ "src", src ]
let em s = tag "em" [] (text s)
let strong s = tag "strong" [] (text s)
let meta attr = tag "meta" attr empty
let spanAttr attr inner = tag "span" attr (flatten inner)
let spanClass class' inner = spanAttr ["class", class'] inner
let span inner = spanAttr [] inner
let italic attr inner = tag "i" attr inner
let p inner = tag "p" [] (flatten inner)
let pClass class' inner = tag "p" ["class", class'] (flatten inner)

let form inner = tag "form" ["method", "POST"] (flatten inner)
let formAttr attr inner = tag "form" (("method", "POST") :: attr) (flatten inner)
let fieldset inner = tag "fieldset" [] (flatten inner)
let legend txt = tag "legend" [] (text txt)
let headerId id inner = tag "header" ["id", id] (flatten inner)
let footer inner = tag "footer" [] (flatten inner)
let submitInput value = inputAttr ["type", "submit"; "value", value]

let tableClass class' inner = tag "table" ["class", class'] (flatten inner)
let tbodyClass class' inner = tag "tbody" ["class", class'] (flatten inner)
let th inner = tag "th" [] (flatten inner)
let thead inner = tag "thead" [] (flatten inner)
let tbody inner = tag "tbody" [] (flatten inner)
let tr inner = tag "tr" [] (flatten inner)
let trClass class' inner = tag "tr" ["class", class'] (flatten inner)
let td inner = tag "td" [] (flatten inner)
let tdAttr attr inner = tag "td" attr (flatten inner)
let labelClass class' inner = tag "label" ["class", class'] (flatten inner)
let buttonClass class' inner = tag "button" ["class", class'] (flatten inner)
let inputAttrInner attr inner = tag "input" attr (flatten inner)
let inputClassPlaceholderName class' placeholder name value inner =
  tag "input" ["class", class'; "placeholder", placeholder; "name", name; "value", value] (flatten inner)
let textareaClassPlaceholder class' placeholder text' = tag "textarea" ["class", class'; "placeholder", placeholder; "rows", "4"] (text text')
let textareaClassPlaceholderName class' name placeholder text' = tag "textarea" ["class", class'; "name", name; "placeholder", placeholder; "rows", "4"] (text text')
let sectionId id inner = tag "section" ["id", id] (flatten inner)

let base_head title' =
  head [
    meta ["charset","utf-8"]
    meta ["name","viewport"; "content","width=device-width, initial-scale=1.0"]
    meta ["name","description"; "content",""]
    meta ["name","author"; "content",""]
    cssLink "http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,400italic,700,800"
    cssLink "http://fonts.googleapis.com/css?family=Raleway:300,200,100"
    link
    title (sprintf "turtle test - %s" title')
    cssLink "/css/bootstrap.min.css"
    cssLink "/css/style.css"
    cssLink "/css/font-awesome.min.css"
  ]
