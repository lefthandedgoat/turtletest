module testrunCreate

open canopy
open canopyExtensions
open runner

let all () =
  context "test run create"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
