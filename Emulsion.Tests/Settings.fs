module Tests

open System
open System.IO

open FSharp.Control.Tasks
open Microsoft.Extensions.Configuration
open Xunit

open Emulsion
open Emulsion.Settings

let private testConfigText = @"{
   ""xmpp"": {
       ""login"": ""login"",
       ""password"": ""password"",
       ""room"": ""room"",
       ""nickname"": ""nickname""
   },
   ""telegram"": {
       ""token"": ""token"",
       ""groupId"": ""groupId""
   }
}"

let private testConfig =
    { xmpp =
        { login = "login"
          password = "password"
          room = "room"
          nickname = "nickname" }
      telegram =
        { token = "token"
          groupId = "groupId" } }

let private mockConfiguration() =
    let path = Path.GetTempFileName()
    task {
        do! File.WriteAllTextAsync(path, testConfigText)
        return ConfigurationBuilder().AddJsonFile(path).Build()
    }


[<Fact>]
let ``Settings read properly`` () =
    task {
        let! configuration = mockConfiguration()
        Assert.Equal(testConfig, Settings.read configuration)
    }