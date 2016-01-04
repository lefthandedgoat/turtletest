module register

open canopy
open canopyExtensions
open runner
open page_register

let all () =
  context "register"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/register"

    "Name" << "fake"
    "Email" << "fake_87654@null.dev"
    "Password" << "test123"
    "Repeat Password" << "test123"
