module testcaseCreate

open canopy
open canopyExtensions
open runner
open page_testcaseCreate

let all () =
  context "test case create"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
