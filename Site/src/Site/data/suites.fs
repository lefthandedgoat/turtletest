module data_suites

open System
open Npgsql
open adohelper
open forms
open types

let connectionString = "Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;"

let toSuite (reader : NpgsqlDataReader) : Suite list =
  [ while reader.Read() do
    yield {
      Id = getInt32 "suite_id" reader
      ApplicationId = getInt32 "application_id" reader
      Name = getString "name" reader
      Version = getString "version" reader
      Owners = getString "owners" reader
      Notes = getString "notes" reader
    }
  ]

let insert application_id (suite : NewSuite) =
  let sql = """
INSERT INTO turtletest.Suites
  (suite_id
   ,application_id
   ,name
   ,version
   ,owners
   ,notes
  ) VALUES (
   DEFAULT
   ,:application_id
   ,:name
   ,:version
   ,:owners
   ,:notes
 ) RETURNING suite_id;
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "application_id" application_id
  |> param "name" suite.Name
  |> param "version" suite.Version
  |> param "owners" suite.Owners
  |> param "notes" suite.Notes
  |> executeScalar
  |> string |> int

let update suite_id (editSuite : forms.EditSuite) =
  let sql = """
UPDATE turtletest.Suites
SET
  application_id = :application_id
  ,name = :name
  ,version = :version
  ,owners = :owners
  ,notes = :notes
WHERE suite_id = :suite_id;
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "application_id" (int editSuite.Application)
  |> param "name" editSuite.Name
  |> param "version" editSuite.Version
  |> param "owners" editSuite.Owners
  |> param "notes" editSuite.Notes
  |> param "suite_id" suite_id
  |> executeNonQuery

let tryById id =
  let sql = """
SELECT * FROM turtletest.Suites
WHERE suite_id = :suite_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "suite_id" id
  |> read toSuite
  |> firstOrNone

let getByUserId id =
  let sql = """
SELECT * FROM turtletest.Suites as s
JOIN turtletest.Applications as a
ON s.application_id = a.application_id
WHERE a.user_id = :user_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "user_id" id
  |> read toSuite

let getByApplicationId application_id =
  let sql = """
SELECT * FROM turtletest.Suites
WHERE application_id = :application_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "application_id" application_id
  |> read toSuite
