module home

open canopy
open canopyExtensions
open common
open runner
open page_home

let all () =
  context "home"

  let mutable username, email = "",""

  once (fun _ ->
    let u, e = page_register.generateUniqueUser()
    username <- u
    email <- e

    page_register.register username email)

  before (fun _ -> goto (page_home.uri username))

  "After new registration, it shows that the person is logged in" &&& fun _ ->
    displayed (_hiName username)

  "After new registration, counts are all 0" &&& fun _ ->
    _applications_count == "0"
    _suites_count == "0"
    _testCases_count == "0"
    _testRuns_count == "0"
