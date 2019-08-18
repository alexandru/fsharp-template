namespace Library.Tests

module Main =
    open System
    open Fake.Core
    open Expecto
    open Expecto.Logging

    [<EntryPoint>]
    let main argv =
        Tests.runTestsInAssembly defaultConfig argv
