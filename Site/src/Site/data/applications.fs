module data_applications

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

let toApplication (reader : NpgsqlDataReader) : Application list =
  [ while reader.Read() do
    yield {
      Name = reader.GetString(reader.GetOrdinal("name"))
      Address = Some <| reader.GetString(reader.GetOrdinal("address"))
      Documentation = Some <| reader.GetString(reader.GetOrdinal("documentation"))
      //Owners = Some <| reader.GetString(reader.GetOrdinal("owners"))
      Owners = None
      Developers = Some <| reader.GetString(reader.GetOrdinal("developers"))
      Notes = Some <| reader.GetString(reader.GetOrdinal("notes"))
    }
  ]

let insert (application : NewApplication) =
  let sql = """
INSERT INTO turtletest.Applications
  (application_id
   ,name
   ,address
   ,documentation
   ,owners
   ,developers
   ,notes
  ) VALUES (
   DEFAULT
   ,:name
   ,:address
   ,:documentation
   ,:owners
   ,:developers
   ,:notes
 ) RETURNING application_id;
"""
  use connection = new NpgsqlConnection("Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;")
  connection.Open()
  use command = new NpgsqlCommand(sql, connection)
  command.Parameters.AddWithValue("name", application.Name) |> ignore
  command.Parameters.AddWithValue("address", application.Address) |> ignore
  command.Parameters.AddWithValue("documentation", application.Documentation) |> ignore
  command.Parameters.AddWithValue("owners", application.Owners) |> ignore
  command.Parameters.AddWithValue("developers", application.Developers) |> ignore
  command.Parameters.AddWithValue("notes", application.Notes) |> ignore
  command.ExecuteScalar() |> string |> int

let getById id =
  sprintf "SELECT * FROM turtletest.applications
  WHERE application_id = %i" id
  |> read toApplication |> List.head
