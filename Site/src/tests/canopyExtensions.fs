module canopyExtensions

open canopy

let private _placeholder value = sprintf "[placeholder = '%s']" value
let placeholder value = _placeholder value |> css

let findByPlaceholder value f =
  try
    f(OpenQA.Selenium.By.CssSelector(_placeholder value)) |> List.ofSeq
  with | ex -> []

let addFinders () =
  addFinder findByPlaceholder

let goto uri = canopy.core.url uri
