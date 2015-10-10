module paths

type StringPath = PrintfFormat<(string -> string),unit,string,string,string>

let root = "/"
let home : StringPath = "/%s/"
let home_link user = sprintf "/%s" user
let applications : StringPath = "/%s/applications"
let applications_link user = sprintf "/%s/applications" user
let suites : StringPath = "/%s/suites"
let suites_link user = sprintf "/%s/suites" user
let testcases : StringPath = "/%s/testcases"
let testcases_link user = sprintf "/%s/testcases" user
let executions : StringPath = "/%s/executions"
let executions_link user = sprintf "/%s/executions" user
