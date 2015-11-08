module data.permissions

open System
open Npgsql
open adohelper
open forms
open types
open types.permissions
open types.session

let connectionString = "Server=127.0.0.1;User Id=turtletest; Password=taconacho;Database=turtletest;"

let insert (permission : Permission) =
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
  |> param "user_id" permission.UserId
  |> param "application_id" permission.ApplicationId
  |> param "permission" (permissionToInt permission.Permission)
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
