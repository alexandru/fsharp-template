# F# Template

A light-weight template built from scratch, starting from:

- [Get started with F# with the .NET Core CLI](https://docs.microsoft.com/en-us/dotnet/fsharp/get-started/get-started-command-line)
- [FAKE: getting started](https://fake.build/fake-gettingstarted.html)
- [Getting Started with Paket](https://fsprojects.github.io/Paket/getting-started.html)

[![CircleCI](https://circleci.com/gh/alexandru/fsharp-template.svg?style=svg)](https://circleci.com/gh/alexandru/fsharp-template)

## Building

To build self-contained deployment that has a native executable:

```sh
dotnet publish -c Release -r osx.10.12-x64 --self-contained true
```

See:

- [Publish .NET Core apps with the CLI](https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli)
- [.NET Core RID Catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog)
