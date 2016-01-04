module application

open canopy
open canopyExtensions
open runner
open page_application

let all () =
  context "application"

  "test 1" &&& fun _ ->
    url "http://localhost:8083/"
