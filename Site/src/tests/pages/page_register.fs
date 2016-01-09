module page_register

open canopy
open canopyExtensions

type Validation =
  | Valid
  | Invalid

let uri = common.baseuri + "register"

let random = System.Random()
let private letters = [ 'a' .. 'z' ]

let private genChars length =
  [| 1 .. length |] |> Array.map (fun _ -> letters.[random.Next(25)]) |> System.String

let name length validation =
  goto uri

  match validation with
  | Valid -> //force failure so we make sure that he error goes away when valid
    click "Submit"
    displayed "Name is required"
    "Name" << genChars length
    click "Submit"
    notDisplayed "Name must be 64 characters or less"
  | Invalid ->
    "Name" << genChars length
    click "Submit"
    displayed "Name must be 64 characters or less"

let password length validation =
  goto uri

  match validation with
  | Valid -> //force failure so we make sure that he error goes away when valid
    "Password" << genChars 1
    click "Submit"
    displayed "Password must be between 6 and 100 characters"
    let password = genChars length
    "Password" << password
    "Repeat Password" << password
    click "Submit"
    notDisplayed "Password must be between 6 and 100 characters"
  | Invalid ->
    "Password" << genChars length
    click "Submit"
    displayed "Password must be between 6 and 100 characters"

let generateUniqueUser () =
  let letters = genChars 10
  let email = sprintf "test_%s@null.dev" letters
  let username = sprintf "test_%s" letters
  username, email

let tryRegister username email =
  goto uri

  "Name" << username
  "Email" << email
  "Password" << "test123"
  "Repeat Password" << "test123"

  click "Submit"

let register username email =
  tryRegister username email
  on (page_home.uri username)
