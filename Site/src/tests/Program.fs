open canopy
open runner

[<EntryPoint>]
let main _ =
  start firefox
  url "http://www.google.com"
  quit()

  canopy.runner.failedCount
