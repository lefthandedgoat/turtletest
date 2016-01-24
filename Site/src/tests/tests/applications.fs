module applications

open canopy
open canopyExtensions
open common
open runner
open page_applications

let all () =
  context "applications"

  let mutable name, email = "",""

  once (fun _ ->
    page_register.generateUniqueUser' &name &email
    page_register.register name email)

  before (fun _ -> goto (page_applications.uri name))

  "When you go there directly, and there is an application, it takes you to the create page" &&& fun _ ->
    on (page_applicationCreate.uri name)

  "After creating an application, it is in the grid" &&& fun _ ->
    page_applicationCreate.createRandom name Public
    goto (page_applications.uri name)
    count rows 1

  "Grid has the right columns" &&& fun _ ->
    count columns 4
    columns *= "Name"
    columns *= "Owners"
    columns *= "Developers"
    columns *= "Private"
