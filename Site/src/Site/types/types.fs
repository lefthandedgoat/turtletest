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
