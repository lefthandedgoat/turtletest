module home

open canopy
open canopyExtensions
open common
open runner
open page_home

let all () =
  context "home"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
