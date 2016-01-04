module testrunEdit

open canopy
open canopyExtensions
open runner
open page_testrunEdit

let all () =
  context "test run edit"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
