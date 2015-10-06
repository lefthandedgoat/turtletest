module fake

open types

let random = System.Random()
let counts () =
  {
    Applications = random.Next(1, 10)
    Suites = random.Next(4, 20)
    TestCases = random.Next(50, 500)
    Executions = random.Next(20, 100)
  }

let executions count apps =
  let appsCount = List.length apps
  let randomApp () = apps.[random.Next(0, appsCount)]
  [1 .. count]
  |> List.map (fun i ->
       {
         Application = randomApp()
         Description = sprintf "Description #%i" i
         Percent = random.Next(1,100)
       })
