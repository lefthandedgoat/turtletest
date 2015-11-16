module data.testruns

open System
open Npgsql
open adohelper
open types.read
open forms.newtypes
open forms.edittypes

let connectionString = "Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;"

let emptyIntArray : int array = [||]

let toTestRun (reader : NpgsqlDataReader) : TestRun list =
  [ while reader.Read() do
    let temp = {
      Id = getInt32 "testrun_id" reader
      ApplicationId = getInt32 "application_id" reader
      RunDate = getDateTime "run_date" reader
      Description = getString "description" reader
      NotRun = getIntArray "not_run" reader
      Passed = getIntArray "passed" reader
      Failed = getIntArray "failed" reader
      PercentRun = 0
    }

    yield { temp with PercentRun = 1 } //todo
  ]

let insert (testrun : NewTestRun) =
  let sql = """
INSERT INTO turtletest.TestRuns
  (testrun_id
   ,application_id
   ,run_date
   ,description
   ,not_run
   ,passed
   ,failed
  ) VALUES (
   DEFAULT
   ,:application_id
   ,:run_date
   ,:description
   ,:not_run
   ,:passed
   ,:failed
 ) RETURNING testrun_id;
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "application_id" (int testrun.Application)
  |> param "run_date" System.DateTime.UtcNow
  |> param "description" testrun.Description
  |> param "not_run" emptyIntArray
  |> param "passed" emptyIntArray
  |> param "failed" emptyIntArray
  |> executeScalar
  |> string |> int

let update testrun_id (editTestRun : EditTestRun) =
  let sql = """
UPDATE turtletest.TestRuns
SET
  application_id = :application_id
  ,description = :description
WHERE testrun_id = :testrun_id;
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "testrun_id" testrun_id
  |> param "application_id" (int editTestRun.Application)
  |> param "description" editTestRun.Description
  |> executeNonQuery

let tryById id =
  let sql = """
SELECT * FROM turtletest.TestRuns
WHERE testrun_id = :testrun_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "testrun_id" id
  |> read toTestRun
  |> firstOrNone

let getByUserId id =
  let sql = """
SELECT * FROM turtletest.TestRuns as t
JOIN turtletest.Applications as a
ON t.application_id = a.application_id
WHERE a.user_id = :user_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "user_id" id
  |> read toTestRun

let getByApplicationId suite_id =
  let sql = """
SELECT * FROM turtletest.TestRuns
WHERE application_id = :application_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "application_id" suite_id
  |> read toTestRun
