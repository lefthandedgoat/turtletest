module canopyExtensions

open canopy

let findByPlaceholder placeholder f =
  try
    let cssSelector = sprintf "[placeholder = '%s']" placeholder
    f(OpenQA.Selenium.By.CssSelector(cssSelector)) |> List.ofSeq
  with | ex -> []

let addFinders () =
  addFinder findByPlaceholder

let goto uri = canopy.core.url uri
