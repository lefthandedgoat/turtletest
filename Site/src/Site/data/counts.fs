module data.counts

open System
open Npgsql
open adohelper
open types.read
open types.session

let connectionString = "Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;"

let toCounts (reader : NpgsqlDataReader) : Counts list =
  [ while reader.Read() do
    yield {
      Applications = getInt32 "applications" reader
      Suites = getInt32 "suites" reader
      TestCases = getInt32 "testcases" reader
      TestRuns = getInt32 "testruns" reader
    }
  ]
let private getPublicCounts userName =
  let sql = """
SELECT
  (SELECT COUNT(a.*) FROM turtletest.Applications as a
  JOIN turtletest.Users as u
  ON a.user_id = u.user_id
  WHERE u.name = :name
  AND a.Private = FALSE) as applications

  ,(SELECT COUNT(s.*) FROM turtletest.Applications as a
  JOIN turtletest.Users as u
  ON a.user_id = u.user_id
  JOIN turtletest.Suites as s
  ON s.application_id = a.application_id
  WHERE u.name = :name
  AND a.Private = FALSE) as suites

  ,(SELECT COUNT(t.*) FROM turtletest.Applications as a
  JOIN turtletest.Users as u
  ON a.user_id = u.user_id
  JOIN turtletest.TestCases as t
  ON t.application_id = a.application_id
  WHERE u.name = :name
  AND a.Private = FALSE) as testcases

  ,(SELECT 0) as testruns
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "name" userName
  |> read toCounts
  |> List.head

let private getPrivateAndPublicCounts userName user_id =
  let sql = """
SELECT
  (SELECT COUNT(a.*) FROM turtletest.Applications as a
  LEFT JOIN turtletest.Permissions as p
  ON a.application_id = p.application_id
  JOIN turtletest.Users as u
  ON a.user_id = u.user_id
  WHERE u.name = :name
  AND a.user_id = :user_id
  AND (a.Private = FALSE
      OR (p.Permission = 1 OR p.Permission = 2))
  ) as applications

  ,(SELECT COUNT(s.*) FROM turtletest.Applications as a
  LEFT JOIN turtletest.Permissions as p
  ON a.application_id = p.application_id
  JOIN turtletest.Users as u
  ON a.user_id = u.user_id
  JOIN turtletest.Suites as s
  ON s.application_id = a.application_id
  WHERE u.name = :name
  AND a.user_id = :user_id
  AND (a.Private = FALSE
      OR (p.Permission = 1 OR p.Permission = 2))
  ) as suites

  ,(SELECT COUNT(t.*) FROM turtletest.Applications as a
  LEFT JOIN turtletest.Permissions as p
  ON a.application_id = p.application_id
  JOIN turtletest.Users as u
  ON a.user_id = u.user_id
  JOIN turtletest.TestCases as t
  ON t.application_id = a.application_id
  WHERE u.name = :name
  AND a.user_id = :user_id
  AND (a.Private = FALSE
      OR (p.Permission = 1 OR p.Permission = 2))
  ) as testcases

  ,(SELECT 0) as testruns
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "name" userName
  |> param "user_id" user_id
  |> read toCounts
  |> List.head

let getCounts userName session =
  match session with
    | NoSession -> getPublicCounts userName
    | User(user_id) -> getPrivateAndPublicCounts userName user_id
