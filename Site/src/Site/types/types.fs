module types

type BCryptScheme =
  {
    Id : int
    WorkFactor : int
  }

type Counts =
  {
    Applications : int
    Suites : int
    TestCases : int
    Executions : int
  }

type ExecutionRow =
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
    ApplicationName : string
    SuiteName : string
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

type RootResponse =
  | Get
  | Success
