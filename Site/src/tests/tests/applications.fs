module applications

open canopy
open canopyExtensions
open runner
open page_applications

let all () =
  context "applications"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
