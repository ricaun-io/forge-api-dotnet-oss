# Autodesk.Forge.Oss

[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](https://github.com/ricaun-io/forge-api-dotnet-oss)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](https://github.com/ricaun-io/forge-api-dotnet-oss/actions/workflows/Build.yml/badge.svg)](https://github.com/ricaun-io/forge-api-dotnet-oss/actions)
[![Nuget](https://img.shields.io/nuget/v/ricaun.Autodesk.Forge.Oss?logo=nuget&label=nuget&color=blue)](https://www.nuget.org/packages/ricaun.Autodesk.Forge.Oss)

## Overview

.NET SDK for **Data Management v2 API** for `Object Storage Service (OSS)`, for more information, please visit  [official documentation](https://aps.autodesk.com/en/docs/data/v2)

### PackageReference
```xml
<PackageReference Include="ricaun.Autodesk.Forge.Oss" Version="*" />
```

### Requirements

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or later
- A registered app on the [Autodesk Platform Service](https://aps.autodesk.com/). 

### Dependencies

- [Autodesk.Forge](Autodesk.Forge) assembly which provides services such as: 
    - Forge [TwoLeggedApi](Autodesk.Forge/Api/TwoLeggedApi.cs)
    - Data Management [BucketsApi](Autodesk.Forge/Api/BucketsApi.cs)
    - Data Management [ObjectsApi](Autodesk.Forge/Api/ObjectsApi.cs)

### Configuration

By default the Forge credentials could be defined with the following environment variables:

```bash
APS_CLIENT_ID=<your client id>
APS_CLIENT_SECRET=<your client secret>
```

or

```bash
FORGE_CLIENT_ID=<your client id>
FORGE_CLIENT_SECRET=<your client secret>
```

## API Reference

The package `ricaun.Autodesk.Forge.Oss` use the namespace `Autodesk.Forge.Oss`.

### OssClient
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

### Bundle 
```csharp
Buckets buckets = await ossClient.GetBucketsAsync();
Bucket bucket = await ossClient.CreateBucketAsync(bucketKey);
Bucket bucket = await ossClient.TryGetBucketDetailsAsync(bucketKey);
Bucket bucket = await ossClient.GetBucketDetailsAsync(bucketKey);
await ossClient.DeleteBucketAsync(bucketKey);
```

### Object 
```csharp
BucketObjects bucketObjects = await ossClient.GetObjectsAsync(bucketKey);
Stream stream = await ossClient.GetObjectAsync(bucketKey, objectName);
ObjectDetails objectDetails = await ossClient.GetObjectDetailsAsync(bucketKey, objectName);
ObjectDetails objectDetails = await ossClient.UploadObjectAsync(bucketKey, objectName);
ObjectDetails objectDetails = await ossClient.GetS3UploadURLAsync(bucketKey, objectName);
ObjectDetails objectDetails = await ossClient.CompleteS3UploadAsync(bucketKey, objectName, uploadKey);
await ossClient.DeleteObjectAsync(bucketKey, objectName);
```

### Signed 
```csharp
PostObjectSigned postObjectSigned = await ossClient.CreateSignedResourceAsync(bucketKey, objectName, postBucketsSigned);
```

### FileExtension 
```csharp
ObjectDetails objectDetails = await ossClient.UploadFileAsync(bucketKey, objectName, localFullName);
await ossClient.DownloadFileAsync(bucketKey, objectName, localFullName);
string signedUrl = await ossClient.CreateSignedFileAsync(bucketKey, objectName);
string signedUrl = await ossClient.CreateSignedFileWriteAsync(bucketKey, objectName);
```

## Release

* [Latest release](https://github.com/ricaun-io/forge-api-dotnet-oss/releases/latest)

## License

This project is [licensed](LICENSE) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](https://github.com/ricaun-io/forge-api-dotnet-oss/stargazers)!