open canopy
open canopyExtensions

[<EntryPoint>]
let main _ =
  canopy.configuration.wipSleep <- 0.2
  canopy.configuration.compareTimeout <- 3.0
  canopy.configuration.elementTimeout <- 3.0
  canopy.configuration.pageTimeout <- 3.0
  addFinders ()

  start firefox

  application.all()
  applications.all()
  applicationCreate.all()
  applicationEdit.all()
  home.all()
  login.all()
  register.all()
  suite.all()
  suites'.all()
  suiteCreate.all()
  suiteEdit.all()
  testcase.all()
  testcases.all()
  testcaseCreate.all()
  testcaseEdit.all()
  testrun.all()
  testruns.all()
  testrunCreate.all()
  testrunEdit.all()

  run()

  System.Console.ReadKey() |> ignore
  quit()

  canopy.runner.failedCount
