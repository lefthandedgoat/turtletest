module home

open canopy
open canopyExtensions
open common
open runner
open page_home

let all () =
  context "home"

  let mutable name, email = "",""

  once (fun _ ->
    page_register.generateUniqueUser' &name &email
    page_register.register name email)

  before (fun _ -> goto (page_home.uri name))

  "After new registration, it shows that the person is logged in" &&& fun _ ->
    displayed (_hiName name)

  "After new registration, counts are all 0" &&& fun _ ->
    _applications_count == "0"
    _suites_count == "0"
    _testCases_count == "0"
    _testRuns_count == "0"

  "After adding an public application, count increases to 1" &&& fun _ ->
    page_applicationCreate.createRandom name Public

    _applications_count == "1"
    _suites_count == "0"
    _testCases_count == "0"
    _testRuns_count == "0"

  "After adding an private application, count increases to 2" &&& fun _ ->
    page_applicationCreate.createRandom name Private

    _applications_count == "2"
    _suites_count == "0"
    _testCases_count == "0"
    _testRuns_count == "0"

  "After logging out, the number of applications should be 1" &&& fun _ ->
    page_login.logout()
    goto (page_home.uri name)

    _applications_count == "1"
    _suites_count == "0"
    _testCases_count == "0"
    _testRuns_count == "0"
