module register

open canopy
open canopyExtensions
open runner
open page_register

let all () =
  context "register"

  before (fun _ -> goto page_register.uri)

  "Name required" &&& fun _ ->
    click "Submit"
    displayed "Name is required"

  "Name invalid" &&& fun _ ->
    name 65 Invalid
    name 66 Invalid

  "Name valid" &&& fun _ ->
    name 1 Valid
    name 2 Valid
    name 63 Valid
    name 64 Valid

  "Email required" &&& fun _ ->
    click "Submit"
    displayed "Email not valid"

  "Email invalid" &&& fun _ ->
    "Email" << "junk"
    click "Submit"

    displayed "Email not valid"

  "Email valid" &&& fun _ ->
    click "Submit"
    displayed "Email not valid"

    "Email" << "junk@null.dev"
    click "Submit"
    notDisplayed "Email not valid"

  "Password required" &&& fun _ ->
    click "Submit"
    displayed "Password must be between 6 and 100 characters"

  "Password invalid" &&& fun _ ->
    password 1 Invalid
    password 2 Invalid
    password 3 Invalid
    password 4 Invalid
    password 5 Invalid
    password 101 Invalid

  "Password valid" &&& fun _ ->
    password 6 Valid
    password 7 Valid
    password 99 Valid
    password 100 Valid

  "Password mismatch" &&& fun _ ->
    "Password" << "123456"
    "Repeat Password" << "654321"
    click "Submit"
    displayed "Passwords must match"

  "Can register new unique user" &&& fun _ ->
    let username, email = generateUniqueUser ()

    "Name" << username
    "Email" << email
    "Password" << "test1234"
    "Repeat Password" << "test1234"
    click "Submit"

    on (page_home.uri username)

  "Duplicate registrations are not allowed" &&& fun _ ->
    let username, email = generateUniqueUser ()

    register username email
    tryRegister username email

    displayed "Name is already taken"
    displayed "Email is already taken"
