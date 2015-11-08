module data.main_page_emails

open System
open Npgsql
open adohelper

let connectionString = "Server=127.0.0.1;User Id=emails; Password=taconacho;Database=emails;"

let insertEmail email =
  let sql = """
INSERT INTO main_page (email)
VALUES (:email)
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "email" email
  |> executeNonQuery
