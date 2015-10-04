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

let home' = "/"
let divId id = divAttr ["id", id]
let divClass c = divAttr ["class", c]
let h1 xml = tag "h1" [] xml
let h2 s = tag "h2" [] (text s)
let aHref href = tag "a" ["href", href]
let aHrefAttr href attr = tag "a" (("href", href) :: attr)
let cssLink href = linkAttr [ "href", href; " rel", "stylesheet"; " type", "text/css" ]
let ul xml = tag "ul" [] (flatten xml)
let ulAttr attr xml = tag "ul" attr (flatten xml)
let li = tag "li" []
let imgSrc src = imgAttr [ "src", src ]
let em s = tag "em" [] (text s)
let strong s = tag "strong" [] (text s)

let form x = tag "form" ["method", "POST"] (flatten x)
let fieldset x = tag "fieldset" [] (flatten x)
let legend txt = tag "legend" [] (text txt)
let submitInput value = inputAttr ["type", "submit"; "value", value]

let table x = tag "table" [] (flatten x)
let th x = tag "th" [] (flatten x)
let tr x = tag "tr" [] (flatten x)
let td x = tag "td" [] (flatten x)

let home'' () =
  html [
    head [
      title "turtle test"
      cssLink "/style.css"
    ]

    body [

      divId "footer" [
        text "built with "
        aHref "http://fsharp.org" (text "F#")
        text " and "
        aHref "http://suave.io" (text "Suave.IO")
      ]
    ]
  ]
  |> xmlToString

let home = OK (home'' ())

let webPart =
  choose [

    GET >>= choose [
      path "/hello" >>= OK "Hello GET"
      path "/" >>= home
      path "/goodbye" >>= OK "Good bye GET"
    ]

    POST >>= choose [
      path "/hello" >>= OK "Hello POST"
      path "/goodbye" >>= OK "Good bye POST"
    ]

    pathRegex "(.*)\.(css|png|gif|js|ico)" >>= Files.browseHome
  ]

startWebServer defaultConfig webPart
