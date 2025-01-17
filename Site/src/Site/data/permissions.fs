module data.permissions

open System
open Npgsql
open adohelper
open types
open types.permissions
open types.session

let connectionString = "Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;"

let toPermission (reader : NpgsqlDataReader) : Permission list =
  [ while reader.Read() do
    yield {
      UserId = getInt32 "user_id" reader
      ApplicationId = getInt32 "application_id" reader
      Permission = getPermission "permission" reader
    }
  ]

let insert user_id application_id permissions =
  let sql = """
INSERT INTO turtletest.Permissions
  (user_id
   ,application_id
   ,permission
  ) VALUES (
   :user_id
   ,:application_id
   ,:permission
 );
"""
  use connection = connection connectionString
  use command = command connection sql
  command
  |> param "user_id" user_id
  |> param "application_id" application_id
  |> param "permission" (permissionToInt permissions)
  |> executeNonQuery

let getPermissionsAndApplications userName session =
  match session with
    | NoSession -> Neither, data.applications.getPublicApplications userName
    | User(user_id) ->
      let user = data.users.getById user_id
      if user.Name = userName
      then Owner, data.applications.getByUserId user_id
      else Contributor, data.applications.getContributorOrPublicApplications user_id

let getApplicationsCreateEditPermissions userName session =
  match session with
    | NoSession -> Neither
    | User(user_id) ->
      let user = data.users.getById user_id
      if user.Name = userName
      then Owner
      else Neither //only owners can create and edit applications

let getSpecificSuiteCreateEditPermissions (suite_id : int) session =
  let getSuitePermission user_id suite_id =
    let sql = """
  SELECT p.*
  FROM turtletest.Suites as s
  JOIN turtletest.Permissions as p
  ON s.application_id = p.application_id
  WHERE s.suite_id = :suite_id
  AND p.user_id = :user_id
  """
    use connection = connection connectionString
    use command = command connection sql
    command
    |> param "suite_id" suite_id
    |> param "user_id" user_id
    |> read toPermission
    |> firstOrNone

  match session with
    | NoSession -> Neither
    | User(user_id) ->
      let permission = getSuitePermission user_id suite_id
      match permission with
        | Some(permission) -> permission.Permission
        | None -> Neither

let getSuiteCreateEditPermissions userName session =
  let getSuitePermission user_id =
    let sql = """
  SELECT p.*
  FROM turtletest.Suites as s
  JOIN turtletest.Permissions as p
  ON s.application_id = p.application_id
  WHERE p.user_id = :user_id
  """
    use connection = connection connectionString
    use command = command connection sql
    command
    |> param "user_id" user_id
    |> read toPermission
    |> firstOrNone

  match session with
    | NoSession -> Neither
    | User(user_id) ->
      let user = data.users.getById user_id
      if user.Name = userName
      then Owner
      else
        let permission = getSuitePermission user_id
        match permission with
          | Some(permission) -> permission.Permission
          | None -> Neither

let getSpecificTestCaseCreateEditPermissions (testcase_id : int) session =
  let getTestCasePermission user_id =
    let sql = """
  SELECT p.*
  FROM turtletest.TestCases as t
  JOIN turtletest.Permissions as p
  ON t.application_id = p.application_id
  WHERE t.testcase_id = :testcase_id
  AND p.user_id = :user_id
  """
    use connection = connection connectionString
    use command = command connection sql
    command
    |> param "testcase_id" testcase_id
    |> param "user_id" user_id
    |> read toPermission
    |> firstOrNone

  match session with
    | NoSession -> Neither
    | User(user_id) ->
      let permission = getTestCasePermission user_id
      match permission with
        | Some(permission) -> permission.Permission
        | None -> Neither

//todo ultimately this should return a list of applications
//or something that the user can do stuff to
let getTestCaseCreateEditPermissions userName session =
  let getTestCasePermission user_id =
    let sql = """
  SELECT p.*
  FROM turtletest.TestCases as t
  JOIN turtletest.Permissions as p
  ON t.application_id = p.application_id
  WHERE p.user_id = :user_id
  """
    use connection = connection connectionString
    use command = command connection sql
    command
    |> param "user_id" user_id
    |> read toPermission
    |> firstOrNone

  match session with
    | NoSession -> Neither
    | User(user_id) ->
      let user = data.users.getById user_id
      if user.Name = userName
      then Owner
      else
        let permission = getTestCasePermission user_id
        match permission with
          | Some(permission) -> permission.Permission
          | None -> Neither

let getSpecificTestRunCreateEditPermissions (testrun_id : int) session =
  let getTestRunPermission user_id =
    let sql = """
  SELECT p.*
  FROM turtletest.TestRuns as t
  JOIN turtletest.Permissions as p
  ON t.application_id = p.application_id
  WHERE t.testrun_id = :testrun_id
  AND p.user_id = :user_id
  """
    use connection = connection connectionString
    use command = command connection sql
    command
    |> param "testrun_id" testrun_id
    |> param "user_id" user_id
    |> read toPermission
    |> firstOrNone

  match session with
    | NoSession -> Neither
    | User(user_id) ->
      let permission = getTestRunPermission user_id
      match permission with
        | Some(permission) -> permission.Permission
        | None -> Neither

//todo ultimately this should return a list of applications
//or something that the user can do stuff to
let getTestRunCreateEditPermissions userName session =
  let getTestRunPermission user_id =
    let sql = """
  SELECT p.*
  FROM turtletest.TestRuns as t
  JOIN turtletest.Permissions as p
  ON t.application_id = p.application_id
  WHERE p.user_id = :user_id
  """
    use connection = connection connectionString
    use command = command connection sql
    command
    |> param "user_id" user_id
    |> read toPermission
    |> firstOrNone

  match session with
    | NoSession -> Neither
    | User(user_id) ->
      let user = data.users.getById user_id
      if user.Name = userName
      then Owner
      else
        let permission = getTestRunPermission user_id
        match permission with
          | Some(permission) -> permission.Permission
          | None -> Neither
