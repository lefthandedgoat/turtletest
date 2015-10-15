module main_page_emails

open System
open Npgsql

let firstOrNone s = s |> Seq.tryFind (fun _ -> true)

let nonQuery sql =
  use connection = new NpgsqlConnection("Server=127.0.0.1;User Id=emails; Password=taconacho;Database=emails;")
  connection.Open()
  use command = new NpgsqlCommand(sql, connection)
  command.ExecuteNonQuery() |> ignore

let insertEmail email =
  sprintf "INSERT INTO main_page (email)
  VALUES ('%s')" email
  |> nonQuery
