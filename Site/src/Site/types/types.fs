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

type Application =
  {
    Name : string;
    Address : string;
    Documentation : string;
    Owners : string;
    Developers : string;
    Notes : string;
  }

type Suite =
  {
    ApplicationName : string;
    Name : string;
    Version : string;
    Owners : string;
    Notes : string;
  }

type RootResponse =
  | Get
  | Success
