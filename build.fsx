#r @"packages\FAKE.1.74.256.0\tools\FakeLib.dll"

open Fake

let buildDir = @".\_build\"
let jsVersion = "1.0"
let cssVersion = "1.0"

// Targets

Target "Noop" (fun () -> trace " *** Noop")

Target "BuildAssets" (fun _ ->
  !! @"Chiffon.msbuild"
    |>  MSBuild buildDir "Build" ["Configuration", "Release"; "CssVersion", cssVersion; "JsVersion", jsVersion]
    |> Log "Build-Output: "
)

// Dependencies

RunTargetOrDefault "Noop"
