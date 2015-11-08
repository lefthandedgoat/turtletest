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

let passwordPattern = @"(\w){6,100}"
let nameRequired = (fun f -> String.IsNullOrWhiteSpace f.Name |> not), "Name", "Name is required"
let nameMaxLength = (fun f -> f.Name.Length <= 64 ), "Name", "Name must be 64 characters or less"
let emailValid = (fun f -> try MailAddress(f.Email)|> ignore; true with | _ -> false), "Email", "Email not valid"
let passwordsMatch = (fun f -> f.Password = f.RepeatPassword), "Password", "Passwords must match"
let passwordRegexMatch =
  (fun f -> Regex(passwordPattern).IsMatch(f.Password) && Regex(passwordPattern).IsMatch(f.RepeatPassword)),
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
  Private : string;
  Address : string;
  Documentation : string;
  Owners : string;
  Developers : string;
  Notes : string;
}

let newApplication : Form<NewApplication> = form

let applicationNameRequired = (fun (app : NewApplication) -> String.IsNullOrWhiteSpace app.Name |> not), "Name", "Name is required"
let applicationPrivateIsBool =
  (fun (app : NewApplication) ->
   let canConvert, _ = System.Boolean.TryParse(app.Private)
   canConvert)
  ,"Private"
  ,"Private must be Yes or No"

let newApplicationValidation newApplication =
  [
    applicationNameRequired
    applicationPrivateIsBool
  ] |> applyValidations newApplication

type NewSuite = {
  Application : string;
  Name : string;
  Version : string;
  Owners : string;
  Notes : string;
}

let newSuite : Form<NewSuite> = form

let applicationRequired = (fun (suite : NewSuite) -> String.IsNullOrWhiteSpace suite.Application |> not), "Application", "Application is required"
let suiteNameRequired = (fun (suite : NewSuite) -> String.IsNullOrWhiteSpace suite.Name |> not), "Name", "Name is required"

let newSuiteValidation newSuite =
  [
    applicationRequired
    suiteNameRequired
  ] |> applyValidations newSuite

type NewTestCase = {
  Application : string;
  Suite : string;
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

let testCaseApplicationRequired = (fun (testCase : NewTestCase) -> String.IsNullOrWhiteSpace testCase.Application |> not), "Application", "Application is required"
let testCaseSuiteRequired = (fun (testCase : NewTestCase) -> String.IsNullOrWhiteSpace testCase.Suite |> not), "Suite", "Suite is required"
let testCaseNameRequired = (fun (testCase : NewTestCase) -> String.IsNullOrWhiteSpace testCase.Name |> not), "Name", "Name is required"

let newTestCaseValidation newTestCase =
  [
    testCaseApplicationRequired
    testCaseSuiteRequired
    testCaseNameRequired
  ] |> applyValidations newTestCase

type EditApplication = {
  Name : string;
  Private : string;
  Address : string;
  Documentation : string;
  Owners : string;
  Developers : string;
  Notes : string;
}

let editApplication : Form<EditApplication> = form

let editApplicationNameRequired = (fun (app : EditApplication) -> String.IsNullOrWhiteSpace app.Name |> not), "Name", "Name is required"
let editApplicationPrivateIsBool =
  (fun (app : EditApplication) ->
   let canConvert, _ = System.Boolean.TryParse(app.Private)
   canConvert)
  ,"Private"
  ,"Private must be Yes or No"

let editApplicationValidation editApplication =
  [
    editApplicationNameRequired
    editApplicationPrivateIsBool
  ] |> applyValidations editApplication

type EditSuite = {
  Application : string;
  Name : string;
  Version : string;
  Owners : string;
  Notes : string;
}

let editSuite : Form<EditSuite> = form

let editSuiteNameRequired = (fun (suite : EditSuite) -> String.IsNullOrWhiteSpace suite.Name |> not), "Name", "Name is required"
let editSuiteApplicationRequired = (fun (suite : EditSuite) -> String.IsNullOrWhiteSpace suite.Name |> not), "Application", "Application is required"
let editSuiteApplicationNumeric =
  (fun (suite : EditSuite) ->
    let canParse, _ = System.Int32.TryParse(suite.Application)
    canParse)
  ,"Application"
  ,"Application is not a valid number"

let editSuiteValidation editSuite =
  [
    editSuiteNameRequired
    editSuiteApplicationRequired
    editSuiteApplicationNumeric
  ] |> applyValidations editSuite
