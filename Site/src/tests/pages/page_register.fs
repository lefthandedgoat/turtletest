module page_register

open canopy
open canopyExtensions

let uri = common.baseuri + "register"

let random = System.Random()

let generateUniqueUser () =
  let letters = [ 'a' .. 'z' ]
  let letters = [| 1 .. 10 |] |> Array.map (fun _ -> letters.[random.Next(25)]) |> System.String
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
