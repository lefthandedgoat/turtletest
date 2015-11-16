module paths

type StringPath = PrintfFormat<(string -> string),unit,string,string,string>
type StringIntPath = PrintfFormat<(string -> int -> string),unit,string,string,string * int>

let withParam (key,value) path = sprintf "%s?%s=%s" path key value

let root = "/"

let register = "/register"

let login = "/login"
let logout = "/logout"

let home : StringPath = "/%s"
let home_link user = sprintf "/%s" user

let application : StringIntPath = "/%s/application/%i"
let application_link user id = sprintf "/%s/application/%i" user id

let applications : StringPath = "/%s/applications"
let applications_link user = sprintf "/%s/applications" user

let applicationCreate : StringPath = "/%s/application/create"
let applicationCreate_link user = sprintf "/%s/application/create" user

let applicationEdit : StringIntPath = "/%s/application/edit/%i"
let applicationEdit_link user id = sprintf "/%s/application/edit/%i" user id

let suite : StringIntPath = "/%s/suite/%i"
let suite_link user id = sprintf "/%s/suite/%i" user id

let suites : StringPath = "/%s/suites"
let suites_link user = sprintf "/%s/suites" user

let suiteCreate : StringPath = "/%s/suite/create"
let suiteCreate_link user = sprintf "/%s/suite/create" user
let suiteCreate_queryStringLink user queryString = sprintf "/%s/suite/create?%s" user queryString

let suiteEdit : StringIntPath = "/%s/suite/edit/%i"
let suiteEdit_link user id = sprintf "/%s/suite/edit/%i" user id

let testcase : StringIntPath = "/%s/testcase/%i"
let testcase_link user id = sprintf "/%s/testcase/%i" user id

let testcases : StringPath = "/%s/testcases"
let testcases_link user = sprintf "/%s/testcases" user

let testcaseCreate : StringPath = "/%s/testcase/create"
let testcaseCreate_link user = sprintf "/%s/testcase/create" user
let testcaseCreate_queryStringlink user queryString = sprintf "/%s/testcase/create?%s" user queryString

let testcaseEdit : StringIntPath = "/%s/testcase/edit/%i"
let testcaseEdit_link user id = sprintf "/%s/testcase/edit/%i" user id

let testrun : StringIntPath = "/%s/testrun/%i"
let testrun_link user id = sprintf "/%s/testrun/%i" user id

let testruns : StringPath = "/%s/testruns"
let testruns_link user = sprintf "/%s/testruns" user

let testrunCreate : StringPath = "/%s/testrun/create"
let testrunCreate_link user = sprintf "/%s/testrun/create" user

let testrunEdit : StringIntPath = "/%s/testrun/edit/%i"
let testrunEdit_link user id = sprintf "/%s/testrun/edit/%i" user id
