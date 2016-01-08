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

  "Email invalid #1" &&& fun _ ->
    click "Submit"

    displayed "Email not valid"

  "Email invalid #2" &&& fun _ ->
    "Email" << "junk"
    click "Submit"

    displayed "Email not valid"

  "Password invalid #1" &&& fun _ ->
    click "Submit"

    displayed "Password must between 6 and 100 characters"

  "Password invalid #1" &&& fun _ ->
    click "Submit"

    displayed "Password must between 6 and 100 characters"

  "Password invalid #2" &&& fun _ ->
    "Password" << "1"
    click "Submit"

    displayed "Password must between 6 and 100 characters"

  "Password invalid #3" &&& fun _ ->
    "Password" << "12"
    click "Submit"

    displayed "Password must between 6 and 100 characters"

  "Password invalid #4" &&& fun _ ->
    "Password" << "123"
    click "Submit"

    displayed "Password must between 6 and 100 characters"

  "Password invalid #5" &&& fun _ ->
    "Password" << "1234"
    click "Submit"

    displayed "Password must between 6 and 100 characters"

  "Password invalid #6" &&& fun _ ->
    "Password" << "12345"
    click "Submit"

    displayed "Password must between 6 and 100 characters"

  "Password invalid #3" &&& fun _ ->
    "Password" << "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901"
    click "Submit"

    displayed "Password must between 6 and 100 characters"

  "Can register new unique user" &&& fun _ ->
    let username, email = generateUniqueUser ()

    "Name" << username
    "Email" << email
    "Password" << "test123"
    "Repeat Password" << "test123"
    click "Submit"

    on (page_home.uri username)

  "Duplicate registrations are not allowed" &&& fun _ ->
    let username, email = generateUniqueUser ()

    register username email
    tryRegister username email

    displayed "Name is already taken"
    displayed "Email is already taken"
