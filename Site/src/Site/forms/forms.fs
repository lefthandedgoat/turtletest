namespace forms

open System.Net.Mail
open Suave.Form
open System
open System.Text.RegularExpressions
open System.Net.Mail

module common =

  let form = Form ([],[])

  let applyValidations form validations =
    validations
    |> List.map (fun (validation, prop, errorMessage) ->
       if validation form |> not
       then Some (prop, errorMessage)
       else None)
    |> List.choose id

module email =

  type InterestedEmail = {
    Email : MailAddress
  }

  let interestedEmail : Form<InterestedEmail> = common.form

module newtypes =

  type NewLoginAttempt = {
    Email : string
    Password : string
  }

  type NewUser = {
    Name : string;
    Email : string;
    Password : string;
    RepeatPassword : string;
  }

  type NewApplication = {
    Name : string;
    Private : string;
    Address : string;
    Documentation : string;
    Owners : string;
    Developers : string;
    Notes : string;
  }

  type NewSuite = {
    Application : string;
    Name : string;
    Version : string;
    Owners : string;
    Notes : string;
  }

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

module newforms =
  open newtypes
  open common

  let loginAttempt : Form<NewLoginAttempt> = form
  let newUser : Form<NewUser> = form
  let newApplication : Form<NewApplication> = form
  let newSuite : Form<NewSuite> = form
  let newTestCase : Form<NewTestCase> = form

module newvalidations =
  open newtypes
  open common

  //NEWUSER
  let passwordPattern = @"(\w){6,100}"
  let nameRequired = (fun (user : NewUser) -> String.IsNullOrWhiteSpace user.Name |> not), "Name", "Name is required"
  let nameMaxLength = (fun (user : NewUser) -> user.Name.Length <= 64 ), "Name", "Name must be 64 characters or less"
  let emailValid = (fun (user : NewUser) -> try MailAddress(user.Email)|> ignore; true with | _ -> false), "Email", "Email not valid"
  let passwordsMatch = (fun (user : NewUser) -> user.Password = user.RepeatPassword), "Password", "Passwords must match"
  let passwordRegexMatch =
    (fun f -> Regex(passwordPattern).IsMatch(f.Password) && Regex(passwordPattern).IsMatch(f.RepeatPassword)),
    "Password",
    "Password must between 6 and 100 characters"

  let newUserValidation newUser =
    [
      nameRequired
      nameMaxLength
      emailValid
      passwordsMatch
      passwordRegexMatch
    ] |> applyValidations newUser

  //NEWAPPLICATION
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

  //NEWSUITE
  let applicationRequired = (fun (suite : NewSuite) -> String.IsNullOrWhiteSpace suite.Application |> not), "Application", "Application is required"
  let suiteNameRequired = (fun (suite : NewSuite) -> String.IsNullOrWhiteSpace suite.Name |> not), "Name", "Name is required"
  let suiteApplicationNumeric =
    (fun (suite : NewSuite) ->
      let canParse, _ = System.Int32.TryParse(suite.Application)
      canParse)
    ,"Application"
    ,"Application is not a valid number"

  let newSuiteValidation newSuite =
    [
      applicationRequired
      suiteNameRequired
      suiteApplicationNumeric
    ] |> applyValidations newSuite

  //NEWTESTCASE
  let testCaseApplicationRequired = (fun (testCase : NewTestCase) -> String.IsNullOrWhiteSpace testCase.Application |> not), "Application", "Application is required"
  let testCaseSuiteRequired = (fun (testCase : NewTestCase) -> String.IsNullOrWhiteSpace testCase.Suite |> not), "Suite", "Suite is required"
  let testCaseNameRequired = (fun (testCase : NewTestCase) -> String.IsNullOrWhiteSpace testCase.Name |> not), "Name", "Name is required"

  let newTestCaseValidation newTestCase =
    [
      testCaseApplicationRequired
      testCaseSuiteRequired
      testCaseNameRequired
    ] |> applyValidations newTestCase

module edittypes =

  type EditApplication = {
    Name : string;
    Private : string;
    Address : string;
    Documentation : string;
    Owners : string;
    Developers : string;
    Notes : string;
  }

  type EditSuite = {
    Application : string;
    Name : string;
    Version : string;
    Owners : string;
    Notes : string;
  }

  type EditTestCase = {
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

module editforms =
  open common
  open edittypes

  let editApplication : Form<EditApplication> = form
  let editSuite : Form<EditSuite> = form
  let editTestCase : Form<EditTestCase> = form

module editvalidations =
  open common
  open edittypes

  //EDITAPPLICATION
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

  //EDITSUITE
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

  //EDITTESTCASE
  let editTestCaseApplicationRequired = (fun (testCase : EditTestCase) -> String.IsNullOrWhiteSpace testCase.Application |> not), "Application", "Application is required"
  let editTestCaseSuiteRequired = (fun (testCase : EditTestCase) -> String.IsNullOrWhiteSpace testCase.Suite |> not), "Suite", "Suite is required"
  let editTestCaseNameRequired = (fun (testCase : EditTestCase) -> String.IsNullOrWhiteSpace testCase.Name |> not), "Name", "Name is required"
  let editTestCaseApplicationNumeric =
    (fun (testCase : EditTestCase) ->
      let canParse, _ = System.Int32.TryParse(testCase.Application)
      canParse)
    ,"Application"
    ,"Application is not a valid number"
  let editTestCaseSuiteNumeric =
    (fun (testCase : EditTestCase) ->
      let canParse, _ = System.Int32.TryParse(testCase.Suite)
      canParse)
    ,"Suite"
    ,"Suite is not a valid number"

  let editTestCaseValidation editTestCase =
    [
      editTestCaseApplicationRequired
      editTestCaseSuiteRequired
      editTestCaseNameRequired
      editTestCaseApplicationNumeric
      editTestCaseSuiteNumeric
    ] |> applyValidations editTestCase
