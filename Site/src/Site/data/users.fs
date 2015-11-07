module data_users

open System
open Npgsql
open adohelper
open forms
open types

let connectionString = "Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;"

let toUser (reader : NpgsqlDataReader) : types.User list =
  [ while reader.Read() do
    yield {
      Id = getInt32 "user_id" reader
      Name = getString "name" reader
      Email = getString "email" reader
    }
  ]

let insert (user : NewUser) =
  let sql = """
INSERT INTO turtletest.Users
  (user_id
   ,name
   ,email
   ,password
   ,salt
   ,scheme
  ) VALUES (
   DEFAULT
   ,:name
   ,:email
   ,:password
   ,:salt
   ,:scheme
 ) RETURNING user_id;
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "name" user.Name
  |> param "email" user.Email
  |> param "password" user.Password
  |> param "salt" "TODO ADD SALT........."
  |> param "scheme" 1 //TODO add scheme and all that
  |> executeScalar
  |> string |> int

//todo : paramaterize to prevent sql injection
let tryByName name =
  let sql = """
SELECT * FROM turtletest.users
WHERE name = :name
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "name" name
  |> read toUser
  |> firstOrNone

//todo all the salting and hashing blah blah
//todo : paramaterize to prevent sql injection
let authenticate email password =
  let sql = """
SELECT * FROM turtletest.users
WHERE email = :email
AND password = :password
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "email" email
  |> param "password" password
  |> read toUser
  |> firstOrNone
