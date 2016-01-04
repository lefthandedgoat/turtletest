module testruns

open canopy
open canopyExtensions
open runner
open page_testruns

let all () =
  context "test runs"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
