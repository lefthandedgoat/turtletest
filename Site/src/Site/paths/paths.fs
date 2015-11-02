module paths

type StringPath = PrintfFormat<(string -> string),unit,string,string,string>
type StringIntPath = PrintfFormat<(string -> int -> string),unit,string,string,string * int>

let root = "/"

let register = "/register"

let home : StringPath = "/%s"
let home_link user = sprintf "/%s" user

let application : StringIntPath = "/%s/application/%i"
let application_link user id = sprintf "/%s/application/%i" user id

let applications : StringPath = "/%s/applications"
let applications_link user = sprintf "/%s/applications" user

let applicationCreate : StringPath = "/%s/application/create"
let applicationCreate_link user = sprintf "/%s/application/create" user

let suites : StringPath = "/%s/suites"
let suites_link user = sprintf "/%s/suites" user

let suitesCreate : StringPath = "/%s/suites/create"
let suitesCreate_link user = sprintf "/%s/suites/create" user

let testcases : StringPath = "/%s/testcases"
let testcases_link user = sprintf "/%s/testcases" user

let testcasesCreate : StringPath = "/%s/testcases/create"
let testcasesCreate_link user = sprintf "/%s/testcases/create" user

let executions : StringPath = "/%s/executions"
let executions_link user = sprintf "/%s/executions" user
