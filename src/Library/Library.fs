module Library

open Newtonsoft.Json

let addNumbers x y = x + y

let getJsonNetJson value =
    sprintf "I used to be %s but now I'm %s thanks to JSON.NET!" value (JsonConvert.SerializeObject(value))
