namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Site")>]
[<assembly: AssemblyProductAttribute("Site")>]
[<assembly: AssemblyDescriptionAttribute("turtletest.com")>]
[<assembly: AssemblyVersionAttribute("0.0.1")>]
[<assembly: AssemblyFileVersionAttribute("0.0.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.1"
