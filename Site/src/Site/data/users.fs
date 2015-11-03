module data_users

open System
open Npgsql
open forms
open types

let firstOrNone s = s |> Seq.tryFind (fun _ -> true)

let read toFunc sql =
  use connection = new NpgsqlConnection("Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;")
  connection.Open()
  use command = new NpgsqlCommand(sql, connection)
  use reader = command.ExecuteReader()
  toFunc reader

let nonQuery sql =
  use connection = new NpgsqlConnection("Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;")
  connection.Open()
  use command = new NpgsqlCommand(sql, connection)
  command.ExecuteNonQuery() |> ignore

let toUser (reader : NpgsqlDataReader) : types.User list =
  [ while reader.Read() do
    yield {
      Id = reader.GetInt32(reader.GetOrdinal("user_id"))
      Name = reader.GetString(reader.GetOrdinal("name"))
      Email = reader.GetString(reader.GetOrdinal("email"))
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
  use connection = new NpgsqlConnection("Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;")
  connection.Open()
  use command = new NpgsqlCommand(sql, connection)
  command.Parameters.AddWithValue("name", user.Name) |> ignore
  command.Parameters.AddWithValue("email", user.Email) |> ignore
  command.Parameters.AddWithValue("password", user.Password) |> ignore
  command.Parameters.AddWithValue("salt", "TODO ADD SALT.........") |> ignore
  command.Parameters.AddWithValue("scheme", 1) |> ignore //TODO add scheme and all that
  command.ExecuteScalar() |> string |> int

//todo : paramaterize to prevent sql injection
let tryByName name =
  sprintf "SELECT * FROM turtletest.users
  WHERE name = '%s'" name
  |> read toUser |> firstOrNone

//todo all the salting and hashing blah blah
//todo : paramaterize to prevent sql injection
let authenticate email password =
  sprintf "SELECT * FROM turtletest.users
  WHERE email = '%s'
  AND password = '%s'" email password
  |> read toUser |> firstOrNone
