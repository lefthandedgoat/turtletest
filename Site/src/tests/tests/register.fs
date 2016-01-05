module register

open canopy
open canopyExtensions
open runner
open page_register

let all () =
  context "register"

  "Can register new unique user" &&& fun _ ->
    goto page_register.uri
    let username, email = generateUniqueUser ()

    "Name" << username
    "Email" << email
    "Password" << "test123"
    "Repeat Password" << "test123"
    click "Submit"

    on (page_home.uri username)
