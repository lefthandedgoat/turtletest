module testcases

open canopy
open canopyExtensions
open runner

let all () =
  context "test cases"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
