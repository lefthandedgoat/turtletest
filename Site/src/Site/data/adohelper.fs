module data.adohelper

open Npgsql
open types.read
open types.permissions

let firstOrNone s = s |> Seq.tryFind (fun _ -> true)

let boolean (value : string) = System.Convert.ToBoolean(value)

let connection (connectionString : string) =
  let connection = new NpgsqlConnection(connectionString)
  connection.Open()
  connection

let command connection sql =
  let command = new NpgsqlCommand(sql, connection)
  command

let param name value (command : NpgsqlCommand) =
  command.Parameters.AddWithValue(name, value) |> ignore
  command

let executeScalar (command : NpgsqlCommand) =
  command.ExecuteScalar()

let executeNonQuery (command : NpgsqlCommand) =
  command.ExecuteNonQuery() |> ignore

let read toFunc (command : NpgsqlCommand) =
  use reader = command.ExecuteReader()
  toFunc reader

let getInt32 name (reader : NpgsqlDataReader) =
  reader.GetInt32(reader.GetOrdinal(name))

let getString name (reader : NpgsqlDataReader) =
  reader.GetString(reader.GetOrdinal(name))

let getBool name (reader : NpgsqlDataReader) =
  reader.GetBoolean(reader.GetOrdinal(name))

let permissionToInt (permission : Permissions) =
  match permission with
    | Owner -> 1
    | Contributor -> 2
    | Neither -> 0
