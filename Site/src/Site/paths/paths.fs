module paths

type StringPath = PrintfFormat<(string -> string),unit,string,string,string>

let root = "/"
let home : StringPath = "/%s/"
let home_link user = sprintf "/%s/" user

let applications : StringPath = "/%s/applications"
let applicationsCreate : StringPath = "/%s/applications/create"
let applications_link user = sprintf "/%s/applications" user
let applicationsCreate_link user = sprintf "/%s/applications/create" user

let suites : StringPath = "/%s/suites"
let suitesCreate : StringPath = "/%s/suites/create"
let suites_link user = sprintf "/%s/suites" user
let suitesCreate_link user = sprintf "/%s/suites/create" user

let testcases : StringPath = "/%s/testcases"
let testcasesCreate : StringPath = "/%s/testcases/create"
let testcases_link user = sprintf "/%s/testcases" user
let testcasesCreate_link user = sprintf "/%s/testcases/create" user

let executions : StringPath = "/%s/executions"
let executions_link user = sprintf "/%s/executions" user
