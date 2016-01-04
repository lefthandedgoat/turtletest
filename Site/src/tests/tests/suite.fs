module suite

open canopy
open canopyExtensions
open runner
open page_suite

let all () =
  context "suite"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
