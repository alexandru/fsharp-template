#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

// File system information
let solutionFile  = "FSharpFun.sln"
// Default target configuration
let configuration =
    DotNet.BuildConfiguration.fromEnvironVarOrDefault
        "configuration"
        DotNet.BuildConfiguration.Release

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs
)

Target.create "Build" (fun _ ->
    let setParams (defaults:DotNet.BuildOptions) =
        { defaults with
            Configuration = configuration
        }

    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build setParams)
)

Target.create "All" ignore

"Clean"
    ==> "Build"
    ==> "All"

Target.runOrDefault "All"
