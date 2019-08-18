#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

let isCI =  Environment.environVarAsBool "CI"

let isRelease (targets : Target list) =
    targets
    |> Seq.map(fun t -> t.Name)
    |> Seq.exists ((=)"Release")

let configuration (targets : Target list) =
    let defaultVal = if isRelease targets then "Release" else "Debug"
    match Environment.environVarOrDefault "CONFIGURATION" defaultVal with
    | "Debug" -> DotNet.BuildConfiguration.Debug
    | "Release" -> DotNet.BuildConfiguration.Release
    | config -> DotNet.BuildConfiguration.Custom config

// // Default target configuration
// let buildConfiguration =
//     DotNet.BuildConfiguration.fromEnvironVarOrDefault
//         "configuration"
//         DotNet.BuildConfiguration.Release

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs
)

Target.create "Build" (fun ctx ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build (fun defaults ->
        { defaults with
            Configuration = configuration (ctx.Context.AllExecutingTargets)
        }
    ))
)

Target.create "Test" <| fun ctx ->
    !! (__SOURCE_DIRECTORY__  @@ "tests/**/*.??proj")
    |> Seq.iter (fun proj ->
        DotNet.test(fun c ->
            let args =
                [

                ] |> String.concat " "
            { c with
                Configuration = configuration (ctx.Context.AllExecutingTargets)
                Common =
                    c.Common
                    |> DotNet.Options.withCustomParams
                        (Some(args))
                }) proj)

Target.create "All" ignore

"Clean"
    ==> "Build"
    =?> ("Test", isCI)
    ==> "All"

Target.runOrDefault "All"
