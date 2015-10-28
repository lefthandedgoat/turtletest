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

let application : Application =
  {
    Name = "Android"
    Address = Some "http://www.android.com"
    Documentation = Some "https://developer.android.com/guide/index.html"
    Owners = None
    Developers = Some "Alex Johnson, Susanne Billings, Tim Duncan"
    Notes = Some "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?"
  }

let suite : Suite =
  {
    ApplicationName = "Android"
    Name = "Android Login"
    Version = Some "1.1"
    Owners = None
    Notes = Some "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?"
  }

let testcase : TestCase =
  {
    ApplicationName = "Android"
    SuiteName = "Android Login"
    Name = "Invalid Username"
    Version = Some "1.1"
    Owners = None
    Notes = Some "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?"
    Requirements = None
    Steps = None
    Expected = None
    History = None
    Attachments = None
  }

let suites =
  [
    ["Trident"; "Internet Explorer 4.0"; "Win 95+"; "4"; "Deprecated"]
    ["Trident"; "Internet Explorer 5.0"; "Win 98+"; "5"; "Deprecated"]
    ["Trident"; "Internet Explorer 6.0"; "XP";      "6"; "Deprecated"]
    ["Trident"; "Internet Explorer 7.0"; "Vista";   "7"; "Deprecated"]
  ]

let testcases =
  [
    ["Trident"; "Internet Explorer 4.0"; "Win 95+"; "4"; "Deprecated"]
    ["Trident"; "Internet Explorer 5.0"; "Win 98+"; "5"; "Deprecated"]
    ["Trident"; "Internet Explorer 6.0"; "XP";      "6"; "Deprecated"]
    ["Trident"; "Internet Explorer 7.0"; "Vista";   "7"; "Deprecated"]
  ]

let applicationsOptions = [1,"Android"; 2,"IOS"; 3,"Desktop"]

let suitesOptions = [1,"Android Login"; 2,"IOS Login"; 3,"Desktop Login"]
