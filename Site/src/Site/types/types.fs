module types

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
    Id : int;
    Name : string;
    Email : string;
  }

type Application =
  {
    Name : string;
    Address : string option;
    Documentation : string option;
    Owners : string option;
    Developers : string  option;
    Notes : string option;
  }

type Suite =
  {
    ApplicationName : string;
    Name : string;
    Version : string option;
    Owners : string option;
    Notes : string option;
  }

type TestCase =
  {
    ApplicationName : string;
    SuiteName : string;
    Name : string;
    Version : string option;
    Owners : string option;
    Notes : string option;
    Requirements : string option;
    Steps : string option;
    Expected : string option;
    History : string option;
    Attachments : string option;
  }

type RootResponse =
  | Get
  | Success
