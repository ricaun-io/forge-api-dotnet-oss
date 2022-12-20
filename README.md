# forge-api-dotnet-oss

[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](../..)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](../../actions/workflows/Build.yml/badge.svg)](../../actions)

## Overview

.NET SDK for **Data Management v2 API** for `Object Storage Service (OSS)`, for more information, please visit  [official documentation](https://aps.autodesk.com/en/docs/data/v2)

### Requirements

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or later
- A registered app on the [Forge Developer Portal](http://forge.autodesk.com). 

### Dependencies

- [Autodesk.Forge](https://github.com/Autodesk-Forge/forge-api-dotnet-client) assembly which provides services such as: 
    - Forge [TwoLeggedApi](https://github.com/Autodesk-Forge/forge-api-dotnet-client/blob/master/src/Autodesk.Forge/Api/TwoLeggedApi.cs)
    - Data Management [BucketsApi](https://github.com/Autodesk-Forge/forge-api-dotnet-client/blob/master/src/Autodesk.Forge/Api/BucketsApi.cs)
    - Data Management [ObjectsApi](https://github.com/Autodesk-Forge/forge-api-dotnet-client/blob/master/src/Autodesk.Forge/Api/ObjectsApi.cs)

### Configuration

By default the Forge credentials could be defined with the following environment variables:

```bash
FORGE_CLIENT_ID=<your client id>
FORGE_CLIENT_SECRET=<your client secret>
```

#### By directly creating API objects

```csharp
using Autodesk.Forge.Oss;
using System;
using System.Threading.Tasks;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var ossClient = new OssClient(new Configuration()
        {
            ClientId = "<your client id>",
            ClientSecret = "<your client secret>"
        });

        var buckets = await ossClient.GetBucketsAsync();
        Console.WriteLine(buckets);
    }
}
```

## Release

* [Latest release](../../releases/latest)

## License

This project is [licensed](LICENSE) under the [MIT Licence](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](../../stargazers)!