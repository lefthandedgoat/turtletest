module suiteEdit

open canopy
open canopyExtensions
open runner
open page_suiteEdit

let all () =
  context "suite create"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
