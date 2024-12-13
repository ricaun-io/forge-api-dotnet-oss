# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.0.0] / 2024-12-13
### Features
- Create `Autodesk.Forge` internal project.
### Autodesk.Forge
- Copy `BucketsApi`, `ObjectsApi`, `TwoLeggedApi` from the original `forge-api-dotnet-client` repository.
- Update deprecated `x-ads-region` to `region` when creating a bucket. [buckets-POST](https://aps.autodesk.com/en/docs/data/v2/reference/http/buckets-POST/)
### Tests
- Add `BucketApi_CreateAndDelete_Region`

## [2.1.0] / 2024-12-12
### Updated
- Remove obsolete endpoints and use `S3`.
- Update `UploadFileAsync` to support multi-part upload.
- Add `APS_CLIENT_ID`, `APS_CLIENT_SECRET` to the `Configuration`.

## [2.0.0] / 2024-08-20
### Features
- Support `APS OAuth V2`.
### Updated
- Update `Autodesk.Forge` to `1.9.9`. (Fix #6)

## [1.0.2] / 2023-08-09
### Updated
- Update `Autodesk.Forge` to `1.9.8` - (Migrate from OAuth V1 to OAuth V2)

## [1.0.1] / 2022-12-26
### Fixed
- Fix `deadlock` when converting async to sync in the `Configuration.GetBearer`.

## [1.0.0] / 2022-12-20
### General
- First Release to Nuget
### Added
- Add `Autodesk.Forge.Oss` project
- Add `Autodesk.Forge.Oss.Tests` project

[vNext]: ../../compare/1.0.0...HEAD
[3.0.0]: ../../compare/2.1.0...3.0.0
[2.1.0]: ../../compare/2.0.0...2.1.0
[2.0.0]: ../../compare/1.0.2...2.0.0
[1.0.2]: ../../compare/1.0.1...1.0.2
[1.0.1]: ../../compare/1.0.0...1.0.1
[1.0.0]: ../../compare/1.0.0