open canopy
open canopyExtensions

[<EntryPoint>]
let main _ =
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
