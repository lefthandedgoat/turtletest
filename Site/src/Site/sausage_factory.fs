module sausage_factory

open System
open Suave
open Suave.Form
open Suave.Cookie
open Suave.Http
open Suave.Http.Redirection
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Http.Applicatives
open Suave.Model.Binding
open Suave.State.CookieStateStore
open Suave.Types
open Suave.Web
open Suave.Utils
open Suave.Http.ServerErrors
open Suave.Html
open html_common
open html_bootstrap
open types.session
open types.read

//todo since we manually handle errors, make bad request log errors and send you too an oops page
let logAndShow500 error =
  printfn "%A" error
  INTERNAL_ERROR views.errors.error_500

let errorHandler (ex : Exception) _ (ctx : HttpContext) =
    printfn "%A" ex
    Response.response HTTP_500 (UTF8.bytes views.errors.error_500) ctx

let bindToForm form handler =
  bindReq (bindForm form) handler logAndShow500

let sessionStore setF = context (fun x ->
  match HttpContext.state x with
  | Some state -> setF state
  | None -> never)

let reset =
  unsetPair Auth.SessionAuthCookie
  >>= unsetPair StateCookie
  >>= FOUND paths.login

let getSession f =
  statefulForSession
  >>= context (fun x ->
    match x |> HttpContext.state with
    | None -> f NoSession
    | Some state ->
      match state.get "user_id" with
      | Some user_id -> f (User user_id)
      | _ -> f NoSession)

let redirectWithReturnPath redirection =
  request (fun x ->
    let path = x.url.AbsolutePath
    Redirection.FOUND (redirection |> paths.withParam ("returnPath", path)))

let userExists userName f_success =
  let user = data.users.tryByName userName
  match user with
    | None -> OK views.errors.error_404
    | Some(user) -> f_success user

let applicationExistsAndUserHasPermissions id f_success (user : User) session =
  let application = data.applications.tryById id
  let permissions = data.permissions.getApplicationsCreateEditPermissions user.Name session
  match application with
    | None -> OK views.errors.error_404
    | Some(application) -> f_success application user permissions session

let loggedOn f_success =
  Auth.authenticate
    Cookie.CookieLife.Session
    false
    (fun () -> Choice2Of2(redirectWithReturnPath paths.login))
    (fun _ -> Choice2Of2 reset)
    f_success

let canCreateEdit f_success user =
  loggedOn (getSession (fun session ->
    match session with
    | User(_) -> f_success user session
    | _ -> UNAUTHORIZED "Not logged in"))

let canView f_success user =
  getSession (fun sess -> f_success user sess)

let getQueryStringValue (req :HttpRequest) queryStringKey =
  match (req.queryParam queryStringKey) with
  | Choice1Of2 value -> value
  | _ -> ""
