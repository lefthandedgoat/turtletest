module login

open canopy
open canopyExtensions
open runner
open page_login

let all () =
  context "login"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
