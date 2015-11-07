module paths

type StringPath = PrintfFormat<(string -> string),unit,string,string,string>
type StringIntPath = PrintfFormat<(string -> int -> string),unit,string,string,string * int>

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

let suiteEdit : StringIntPath = "/%s/suite/edit/%i"
let suiteEdit_link user id = sprintf "/%s/suite/edit/%i" user id

let testcases : StringPath = "/%s/testcases"
let testcases_link user = sprintf "/%s/testcases" user

let testcasesCreate : StringPath = "/%s/testcases/create"
let testcasesCreate_link user = sprintf "/%s/testcases/create" user

let executions : StringPath = "/%s/executions"
let executions_link user = sprintf "/%s/executions" user
