module page_applications

open canopy
open canopyExtensions

let uri name = sprintf "%s%s/applications" common.baseuri name

let _rows = css "tbody tr"
let _columns = css "th"
let _create = text "Create"
