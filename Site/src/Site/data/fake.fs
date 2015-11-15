module data.fake

open types.read

let empty = ""
let random = System.Random()

let testruns count apps =
  let appsCount = List.length apps
  let randomApp () = apps.[random.Next(0, appsCount)]
  [1 .. count]
  |> List.map (fun i ->
       {
         Application = randomApp()
         Description = sprintf "Description #%i" i
         Percent = random.Next(1,100)
       })
