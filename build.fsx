#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

let runDotNet cmd workingDir =
    async {
        let result =
            DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""
        if result.ExitCode <> 0 then
            failwithf "'dotnet %s' failed in %s" cmd workingDir
    }

let libraryPath = Path.getFullName "./tests/Library.Tests"
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

Target.create "clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ "tests/**/bin"
    ++ "tests/**/obj"
    |> Shell.cleanDirs
)

Target.create "build" (fun ctx ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build (fun defaults ->
        { defaults with
            Configuration = configuration (ctx.Context.AllExecutingTargets)
        }
    ))
)

Target.create "test-watch" <| fun _ ->
    !! "tests/**/*.*proj"
    |> Seq.map (Path.getDirectory >> runDotNet "watch test")
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore

Target.create "test" <| fun ctx ->
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

Target.create "all" ignore

"clean"
    ==> "build"
    =?> ("test", isCI)
    ==> "all"

"clean"
    ==> "test-watch"

Target.runOrDefault "all"
