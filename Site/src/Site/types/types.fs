namespace types

module crypto =
  type BCryptScheme =
    {
      Id : int
      WorkFactor : int
    }

module session =
  type Session =
    | NoSession
    | User of int

module response =
  type RootResponse =
    | Get
    | Success

module permissions =
  type Permissions =
    | Owner
    | Contributor
    | Neither

  let ownerOrContributor permissions =
    match permissions with
      | Owner
      | Contributor -> true
      | Neither -> false

  let owner permissions =
    match permissions with
      | Owner -> true
      | Contributor
      | Neither -> false

  type Permission =
    {
      UserId : int
      ApplicationId : int
      Permission : Permissions
    }

module read =
  type Counts =
    {
      Applications : int
      Suites : int
      TestCases : int
      TestRuns : int
    }

  type TestRunRow =
    {
      Application : string
      Description : string
      Percent : int
    }

  type User =
    {
      Id : int
      Name : string
      Email : string
      Password : string
      Scheme : int
    }

  type Application =
    {
      Id : int
      Private : bool
      Name : string
      Address : string
      Documentation : string
      Owners : string
      Developers : string
      Notes : string
    }

  type Suite =
    {
      Id : int
      ApplicationId : int
      Name : string
      Version : string
      Owners : string
      Notes : string
    }

  type TestCase =
    {
      Id : int
      ApplicationId : int
      SuiteId : int
      Name : string
      Version : string
      Owners : string
      Notes : string
      Requirements : string
      Steps : string
      Expected : string
      History : string
      Attachments : string
    }

  type TestRun =
    {
      Id : int
      ApplicationId : int
      RunDate : System.DateTime
      Description : string
      NotRun : int array
      Passed : int array
      Failed : int array
      PercentRun : int
    }

namespace forms
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

  type NewTestRun = {
    Application : string
    Description : string
    TestCases : string
  }

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

  type EditTestRun = {
    Application : string
    Description : string
  }
