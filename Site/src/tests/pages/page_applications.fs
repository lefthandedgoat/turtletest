module page_applications

open canopy
open canopyExtensions

let uri name = sprintf "%s%s/applications" common.baseuri name

let rows = css "tbody tr"
let columns = css "th"
