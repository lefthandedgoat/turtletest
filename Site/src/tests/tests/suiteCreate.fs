module suiteCreate

open canopy
open canopyExtensions
open runner
open page_suiteCreate

let all () =
  context "suite create"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
