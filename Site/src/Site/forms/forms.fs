module forms

open System.Net.Mail
open Suave.Form
open System
open System.Text.RegularExpressions
open System.Net.Mail

type LoginAttempt = {
  Email : string
  Password : string
}

let loginAttempt : Form<LoginAttempt> = Form ([],[])

type InterestedEmail = {
  Email : MailAddress
}

let interestedEmail : Form<InterestedEmail> = Form ([],[])

type NewUser = {
  Name : string;
  Email : string;
  Password : string;
  RepeatPassword : string;
}

let pattern = @"(\w){6,100}"
let nameRequired = (fun f -> String.IsNullOrWhiteSpace f.Name |> not), "Name is required"
let nameMaxLength = (fun f -> f.Name.Length <= 64 ), "Name must be 64 characters or less"
let emailValid = (fun f -> try MailAddress(f.Email)|> ignore; true with | _ -> false), "Email not valid"
let passwordsMatch = (fun f -> f.Password = f.RepeatPassword), "Passwords must match"
let passwordRegexMatch =
  (fun f -> Regex(pattern).IsMatch(f.Password) && Regex(pattern).IsMatch(f.RepeatPassword)),
  "Password must between 6 and 100 characters"

let newUser : Form<NewUser> =
  Form ([],
    [
      nameRequired
      nameMaxLength
      emailValid
      passwordsMatch
      passwordRegexMatch
    ])

type NewApplication = {
  Name : string;
  Address : string;
  Documentation : string;
  Owners : string;
  Developers : string;
  Notes : string;
}

let newApplication : Form<NewApplication> = Form ([],[])

type NewSuite = {
  Application : string;
  Name : string;
  Version : string;
  Owners : string;
  Notes : string;
}

let newSuite : Form<NewSuite> = Form ([],[])

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

let newTestCase : Form<NewTestCase> = Form ([],[])
