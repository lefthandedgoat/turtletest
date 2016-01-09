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

module newforms =
  open forms.newtypes
  open common

  let loginAttempt : Form<NewLoginAttempt> = form
  let newUser : Form<NewUser> = form
  let newApplication : Form<NewApplication> = form
  let newSuite : Form<NewSuite> = form
  let newTestCase : Form<NewTestCase> = form
  let newTestRun : Form<NewTestRun> = form

module newvalidations =
  open forms.newtypes
  open common

  //NEWUSER
  let passwordPattern = @"(\w){6,100}"
  let nameRequired = (fun (user : NewUser) -> String.IsNullOrWhiteSpace user.Name |> not), "Name", "Name is required"
  let nameMaxLength = (fun (user : NewUser) -> user.Name.Length <= 64 ), "Name", "Name must be 64 characters or less"
  let emailValid = (fun (user : NewUser) -> try MailAddress(user.Email)|> ignore; true with | _ -> false), "Email", "Email not valid"
  let passwordsMatch = (fun (user : NewUser) -> user.Password = user.RepeatPassword), "Password", "Passwords must match"
  let passwordRegexMatch =
    (fun f -> Regex(passwordPattern).IsMatch(f.Password) && Regex(passwordPattern).IsMatch(f.RepeatPassword))
    ,"Password"
    ,"Password must be between 6 and 100 characters"
  let nameUnique =
    (fun (user : NewUser) ->
      let tryUser = data.users.tryByName user.Name
      match tryUser with
        | None -> true
        | Some(_) -> false)
    , "Name"
    , "Name is already taken"

  let emailUnique =
    (fun (user : NewUser) ->
      let tryUser = data.users.tryByEmail user.Email
      match tryUser with
        | None -> true
        | Some(_) -> false)
    , "Email"
    , "Email is already taken"

  let newUserValidation newUser =
    [
      nameRequired
      nameMaxLength
      emailValid
      passwordsMatch
      passwordRegexMatch
      nameUnique
      emailUnique
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
  let applicationRequired =
    (fun (suite : NewSuite) ->
      let canParse, _ = System.Int32.TryParse(suite.Application)
      canParse && (not <| String.IsNullOrWhiteSpace suite.Application))
    ,"Application"
    ,"Application is required"
  let suiteNameRequired = (fun (suite : NewSuite) -> String.IsNullOrWhiteSpace suite.Name |> not), "Name", "Name is required"

  let newSuiteValidation newSuite =
    [
      applicationRequired
      suiteNameRequired
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

  //NEWTESTRUN
  let testRunApplicationRequired = (fun (testRun : NewTestRun) -> String.IsNullOrWhiteSpace testRun.Application |> not), "Application", "Application is required"
  let testRunDescriptionRequired = (fun (testRun : NewTestRun) -> String.IsNullOrWhiteSpace testRun.Description |> not), "Description", "Description is required"
  let testRunTestCasesRequired = (fun (testRun : NewTestRun) -> String.IsNullOrWhiteSpace testRun.TestCases |> not), "Test Cases", "Test Cases are required"

  let newTestRunValidation newTestRun =
    [
      testRunApplicationRequired
      testRunDescriptionRequired
      testRunTestCasesRequired
    ] |> applyValidations newTestRun

module editforms =
  open forms.edittypes
  open common

  let editApplication : Form<EditApplication> = form
  let editSuite : Form<EditSuite> = form
  let editTestCase : Form<EditTestCase> = form
  let editTestRun : Form<EditTestRun> = form

module editvalidations =
  open forms.edittypes
  open common

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
  let editSuiteApplicationRequired =
    (fun (suite : EditSuite) ->
      let canParse, _ = System.Int32.TryParse(suite.Application)
      canParse && (not <| String.IsNullOrWhiteSpace suite.Application))
    ,"Application"
    ,"Application is required"

  let editSuiteValidation editSuite =
    [
      editSuiteNameRequired
      editSuiteApplicationRequired
    ] |> applyValidations editSuite

  //EDITTESTCASE
  let editTestCaseApplicationRequired =
    (fun (testCase : EditTestCase) ->
      let canParse, _ = System.Int32.TryParse(testCase.Application)
      canParse && (not <| String.IsNullOrWhiteSpace testCase.Application))
    ,"Application"
    ,"Application is required"
  let editTestCaseSuiteRequired =
    (fun (testCase : EditTestCase) ->
      let canParse, _ = System.Int32.TryParse(testCase.Suite)
      canParse && (String.IsNullOrWhiteSpace testCase.Suite |> not))
    ,"Suite"
    ,"Suite is required"
  let editTestCaseNameRequired = (fun (testCase : EditTestCase) -> String.IsNullOrWhiteSpace testCase.Name |> not), "Name", "Name is required"

  let editTestCaseValidation editTestCase =
    [
      editTestCaseApplicationRequired
      editTestCaseSuiteRequired
      editTestCaseNameRequired
    ] |> applyValidations editTestCase

  //EDITTESTRUN
  let editTestRunApplicationRequired =
    (fun (testRun : EditTestRun) ->
      let canParse, _ = System.Int32.TryParse(testRun.Application)
      canParse && (not <| String.IsNullOrWhiteSpace testRun.Application))
    ,"Application"
    ,"Application is required"
  let editTestRunNameRequired = (fun (testRun : EditTestRun) -> String.IsNullOrWhiteSpace testRun.Description |> not), "Description", "Description is required"

  let editTestRunValidation editTestRun =
    [
      editTestRunApplicationRequired
      editTestRunNameRequired
    ] |> applyValidations editTestRun
