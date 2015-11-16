module data.testcases

open System
open Npgsql
open adohelper
open types.read
open forms.newtypes
open forms.edittypes

let connectionString = "Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;"

let toTestCase (reader : NpgsqlDataReader) : TestCase list =
  [ while reader.Read() do
    yield {
      Id = getInt32 "testcase_id" reader
      ApplicationId = getInt32 "application_id" reader
      SuiteId = getInt32 "suite_id" reader
      Name = getString "name" reader
      Version = getString "version" reader
      Owners = getString "owners" reader
      Notes = getString "notes" reader
      Requirements = getString "requirements" reader
      Steps = getString "steps" reader
      Expected = getString "expected" reader
      History = getString "history" reader
      Attachments = getString "attachments" reader
    }
  ]

let insert (testcase : NewTestCase) =
  let sql = """
INSERT INTO turtletest.TestCases
  (testcase_id
   ,application_id
   ,suite_id
   ,name
   ,version
   ,owners
   ,notes
   ,requirements
   ,steps
   ,expected
   ,history
   ,attachments
  ) VALUES (
   DEFAULT
   ,:application_id
   ,:suite_id
   ,:name
   ,:version
   ,:owners
   ,:notes
   ,:requirements
   ,:steps
   ,:expected
   ,:history
   ,:attachments
 ) RETURNING testcase_id;
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "application_id" (int testcase.Application)
  |> param "suite_id" (int testcase.Suite)
  |> param "name" testcase.Name
  |> param "version" testcase.Version
  |> param "owners" testcase.Owners
  |> param "notes" testcase.Notes
  |> param "requirements" testcase.Requirements
  |> param "steps" testcase.Steps
  |> param "expected" testcase.Expected
  |> param "history" testcase.History
  |> param "attachments" testcase.Attachments
  |> executeScalar
  |> string |> int

let update testcase_id (editTestCase : EditTestCase) =
  let sql = """
UPDATE turtletest.TestCases
SET
  application_id = :application_id
  ,suite_id = :suite_id
  ,name = :name
  ,version = :version
  ,owners = :owners
  ,notes = :notes
  ,requirements = :requirements
  ,steps = :steps
  ,expected = :expected
  ,history = :history
  ,attachments = :attachments
WHERE testcase_id = :testcase_id;
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "application_id" (int editTestCase.Application)
  |> param "suite_id" (int editTestCase.Suite)
  |> param "name" editTestCase.Name
  |> param "version" editTestCase.Version
  |> param "owners" editTestCase.Owners
  |> param "notes" editTestCase.Notes
  |> param "requirements" editTestCase.Requirements
  |> param "steps" editTestCase.Steps
  |> param "expected" editTestCase.Expected
  |> param "history" editTestCase.History
  |> param "attachments" editTestCase.Attachments
  |> param "testcase_id" testcase_id
  |> executeNonQuery

let tryById id =
  let sql = """
SELECT * FROM turtletest.TestCases
WHERE testCase_id = :testCase_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "testcase_id" id
  |> read toTestCase
  |> firstOrNone

let getByUserId id =
  let sql = """
SELECT * FROM turtletest.TestCases as t
JOIN turtletest.Applications as a
ON t.application_id = a.application_id
WHERE a.user_id = :user_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "user_id" id
  |> read toTestCase

let getBySuiteId suite_id =
  let sql = """
SELECT * FROM turtletest.TestCases
WHERE suite_id = :suite_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "suite_id" suite_id
  |> read toTestCase
