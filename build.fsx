// include Fake lib
#r @"packages\FAKE.1.74.255.0\tools\FakeLib.dll"

open Fake

// Directories
let buildDir  = @".\build\"
let publishDir = @".\_build\"
let tmpDir = @".\tmp\"

let version = "0.1"

// Targets
Target "Clean" (fun _ -> 
    CleanDirs [buildDir; tmpDir]
)

Target "Build" (fun _ ->    
    !! @"src\*.csproj" 
      |> MSBuildDebug buildDir "Build" 
      |> Log "Build-Output: "
)

Target "Publish" (fun _ ->    
    !! @"src\*.csproj" 
      |> MSBuildDebug publishDir "Publish" 
      |> Log "Publish-Output: "
)

Target "Zip" (fun _ ->
    !+ (buildDir + "\**\*.*") 
        -- "*.zip" 
        |> Scan
        |> Zip buildDir (tmpDir + "Chiffon." + version + ".zip")
)

// Dependencies
"Clean"
  ==> "Build" 
  ==> "Zip"
 
Run <| getBuildParamOrDefault "target" "Clean"