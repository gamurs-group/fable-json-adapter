# Gamurs.Fable.SimpleJson

A simple pipeline-based Fable API for adapting and validating POJOs into F# types.

### Nuget Packages

Stable | Prerelease
--- | ---
[![NuGet Badge](https://buildstats.info/nuget/Gamurs.Fable.SimpleJson)](https://www.nuget.org/packages/Gamurs.Fable.SimpleJson/) | [![NuGet Badge](https://buildstats.info/nuget/Gamurs.Fable.SimpleJson?includePreReleases=true)](https://www.nuget.org/packages/Gamurs.Fable.SimpleJson/)


## Example

```
module Example

open Gamurs.Fable.JsonAdapter

/// Get the the total number of search hits from a search response
let getTotalSearchHits (response : obj) : int =
    response
    |> JsonAdapter.getObj "hits"
    |> JsonAdapter.getInt "total"

```

## Development

### Building

Make sure the following **requirements** are installed in your system:

* [dotnet SDK](https://www.microsoft.com/net/download/core) 2.0 or higher
* [node.js](https://nodejs.org) 6.11 or higher
* [yarn](https://yarnpkg.com)
* [Mono](http://www.mono-project.com/) if you're on Linux or macOS.

Then you just need to type `./build.cmd` or `./build.sh`

### Release

In order to push the package to [nuget.org](https://nuget.org) you need to add your API keys to `NUGET_KEY` environmental variable.
You can create a key [here](https://www.nuget.org/account/ApiKeys).

- Update RELEASE_NOTES with a new version, data and release notes [ReleaseNotesHelper](http://fake.build/apidocs/fake-releasenoteshelper.html).
Ex:

```
#### 0.2.0 - 30.04.2017
* FEATURE: Does cool stuff!
* BUGFIX: Fixes that silly oversight
```

- You can then use the Release target. This will:
  - make a commit bumping the version: Bump version to 0.2.0
  - publish the package to nuget
  - push a git tag

`./build.sh Release`
