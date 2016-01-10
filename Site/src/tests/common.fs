module common

let baseuri = "http://localhost:8083/"

type Validation = Valid | Invalid

type Privacy = Public | Private

let random = System.Random()
let private letters = [ 'a' .. 'z' ]

let genChars length =
  [| 1 .. length |] |> Array.map (fun _ -> letters.[random.Next(25)]) |> System.String
