module data.users

open System
open Npgsql
open adohelper
open forms.newtypes
open types.crypto
open types.read
open BCrypt.Net

let connectionString = "Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;"

let bCryptSchemes : BCryptScheme list = [ { Id = 1; WorkFactor = 8; } ]
let getBCryptScheme id = bCryptSchemes |> List.find (fun scheme -> scheme.Id = id)
let currentBCryptScheme = 1

let toUser (reader : NpgsqlDataReader) : types.read.User list =
  [ while reader.Read() do
    yield {
      Id = getInt32 "user_id" reader
      Name = getString "name" reader
      Email = getString "email" reader
      Password = getString "Password" reader
      Scheme = getInt32 "scheme" reader
    }
  ]

let insert (user : NewUser) =
  let sql = """
INSERT INTO turtletest.Users
  (user_id
   ,name
   ,email
   ,password
   ,scheme
  ) VALUES (
   DEFAULT
   ,:name
   ,:email
   ,:password
   ,:scheme
 ) RETURNING user_id;
"""
  let bCryptScheme = getBCryptScheme currentBCryptScheme
  let salt = BCrypt.GenerateSalt(bCryptScheme.WorkFactor)
  let password = BCrypt.HashPassword(user.Password, salt)

  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "name" user.Name
  |> param "email" user.Email
  |> param "password" password
  |> param "scheme" bCryptScheme.Id
  |> executeScalar
  |> string |> int

let getById user_id =
  let sql = """
SELECT * FROM turtletest.users
WHERE user_id = :user_id
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "user_id" user_id
  |> read toUser
  |> List.head

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

//todo add index on email
let tryByEmail email =
  let sql = """
SELECT * FROM turtletest.users
WHERE email = :email
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "email" email
  |> read toUser
  |> firstOrNone

let authenticate email password =
  let sql = """
SELECT * FROM turtletest.users
WHERE email = :email
"""
  use connection = connection connectionString
  use command = command connection sql
  let user =
    command
    |> param "email" email
    |> read toUser
    |> firstOrNone
  match user with
    | None -> None
    | Some(user) ->
      let verified = BCrypt.Verify(password, user.Password)
      if verified
      then Some(user)
      else None
