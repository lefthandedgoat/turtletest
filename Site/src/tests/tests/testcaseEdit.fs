module testcaseEdit

open canopy
open canopyExtensions
open runner
open page_testcaseEdit

let all () =
  context "test case edit"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
