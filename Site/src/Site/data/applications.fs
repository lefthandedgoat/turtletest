module data_applications

open System
open Npgsql
open adohelper
open forms
open types

let connectionString = "Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;"

let toApplication (reader : NpgsqlDataReader) : Application list =
  [ while reader.Read() do
    yield {
      Id = getInt32 "application_id" reader
      Name = getString "name" reader
      Address = getString "address" reader
      Documentation = getString "documentation" reader
      Owners = getString "owners" reader
      Developers = getString "developers" reader
      Notes = getString "notes" reader
    }
  ]

let insert user_id (application : NewApplication) =
  let sql = """
INSERT INTO turtletest.Applications
  (application_id
   ,user_id
   ,name
   ,address
   ,documentation
   ,owners
   ,developers
   ,notes
  ) VALUES (
   DEFAULT
   ,:user_id
   ,:name
   ,:address
   ,:documentation
   ,:owners
   ,:developers
   ,:notes
 ) RETURNING application_id;
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "name" application.Name
  |> param "user_id" user_id
  |> param "address" application.Address
  |> param "documentation" application.Documentation
  |> param "owners" application.Owners
  |> param "developers" application.Developers
  |> param "notes" application.Notes
  |> executeScalar
  |> string |> int

let update application_id (editApplication : forms.EditApplication) =
  let sql = """
UPDATE turtletest.Applications
SET
  name = :name
  ,address = :address
  ,documentation = :documentation
  ,owners = :owners
  ,developers = :developers
  ,notes = :notes
WHERE application_id = :application_id;
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "name" editApplication.Name
  |> param "address" editApplication.Address
  |> param "documentation" editApplication.Documentation
  |> param "owners" editApplication.Owners
  |> param "developers" editApplication.Developers
  |> param "notes" editApplication.Notes
  |> param "application_id" application_id
  |> executeNonQuery

let getById id =
  let sql = """
SELECT * FROM turtletest.applications
WHERE application_id = :application_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "application_id" id
  |> read toApplication
  |> firstOrNone

let getByUserId user_id =
  let sql = """
SELECT * FROM turtletest.applications
WHERE user_id = :user_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "user_id" user_id
  |> read toApplication
