module forms

open System.Net.Mail
open Suave.Form
open System
open System.Text.RegularExpressions
open System.Net.Mail

let form = Form ([],[])

let applyValidations form validations =
  validations
  |> List.map (fun (validation, prop, errorMessage) ->
     if validation form |> not
     then Some (prop, errorMessage)
     else None)
  |> List.choose id

type LoginAttempt = {
  Email : string
  Password : string
}

let loginAttempt : Form<LoginAttempt> = Form ([],[])

type InterestedEmail = {
  Email : MailAddress
}

let interestedEmail : Form<InterestedEmail> = form

type NewUser = {
  Name : string;
  Email : string;
  Password : string;
  RepeatPassword : string;
}

let pattern = @"(\w){6,100}"
let nameRequired = (fun f -> String.IsNullOrWhiteSpace f.Name |> not), "Name", "Name is required"
let nameMaxLength = (fun f -> f.Name.Length <= 64 ), "Name", "Name must be 64 characters or less"
let emailValid = (fun f -> try MailAddress(f.Email)|> ignore; true with | _ -> false), "Email", "Email not valid"
let passwordsMatch = (fun f -> f.Password = f.RepeatPassword), "Password", "Passwords must match"
let passwordRegexMatch =
  (fun f -> Regex(pattern).IsMatch(f.Password) && Regex(pattern).IsMatch(f.RepeatPassword)),
  "Password",
  "Password must between 6 and 100 characters"

let newUser : Form<NewUser> = form

let newUserValidation newUser =
  [
    nameRequired
    nameMaxLength
    emailValid
    passwordsMatch
    passwordRegexMatch
  ] |> applyValidations newUser

type NewApplication = {
  Name : string;
  Address : string;
  Documentation : string;
  Owners : string;
  Developers : string;
  Notes : string;
}

let newApplication : Form<NewApplication> = form

type NewSuite = {
  Application : string;
  Name : string;
  Version : string;
  Owners : string;
  Notes : string;
}

let newSuite : Form<NewSuite> = form

type NewTestCase = {
  Application : string;
  Name : string;
  Version : string;
  Owners : string;
  Notes : string;
  Requirements : string;
  Steps : string;
  Expected : string;
  History : string;
  Attachments : string;
}

let newTestCase : Form<NewTestCase> = form
