// The result of: paket generate-load-scripts
#load "./.paket/load/netstandard2.0/Newtonsoft.Json.fsx"
#load "./src/Library/Library.fs"
open FSharp.Core
open Library

// ...
printfn "%s" (getJsonNetJson "value")
