open canopy
open canopyExtensions
open types

[<EntryPoint>]
let main _ =
  configuration.wipSleep <- 0.2
  configuration.compareTimeout <- 3.0
  configuration.elementTimeout <- 3.0
  configuration.pageTimeout <- 3.0
  configuration.failFast := true

  addFinders ()

  start firefox
  pin FullScreen

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
