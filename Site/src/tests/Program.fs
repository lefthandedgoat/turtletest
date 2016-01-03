open canopy
open runner

[<EntryPoint>]
let main _ =
  start firefox
  url "http://localhost:8083"
  quit()

  canopy.runner.failedCount
