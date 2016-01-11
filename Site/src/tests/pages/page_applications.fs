module page_applications

open canopy
open canopyExtensions

let uri name = sprintf "%s%s/applications" common.baseuri name
