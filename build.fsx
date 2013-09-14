// include Fake lib
#r @"packages\FAKE.1.74.256.0\tools\FakeLib.dll"

open Fake

let buildDir = @".\_build\"

// Targets

Target "Noop" (fun () -> trace " *** Noop")

Target "Clean" (fun _ -> trace " *** Clean")

Target "Build" (fun _ ->
  !! @"Chiffon.msbuild"
    |> MSBuildRelease buildDir "Clean"
    |> Log "Build-Output: "
)

Target "Zip" (fun _ -> trace " *** Zip")

Target "Publish" (fun _ -> trace " *** Publish")

Target "Deploy" (fun _ -> trace " *** Deploy")

// Dependencies

"Clean"
  ==> "Build"
  ==> "Zip"

RunTargetOrDefault "Noop"

//Run <| getBuildParamOrDefault "target" "Clean"