# Changelog

- All notable changes to this package will be documented in this file.
- The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) and this package adheres to [Semantic Versioning](https://semver.org/)

## [3.1.3] - 2022-05-23
### Updated
- Updated dependencies.
### Added
- Added check for illegal characters.

## [3.1.2] - 2022-04-25
## Fixed
- Issue with ArgumentExceptions being thrown when trying to load assembly files on newer Unity versions.

## [3.1.1] - 2022-03-31
## Fixed
- Issue with FileNotFoundExceptions sometimes being thrown when trying to load an assembly file.

## [3.1.0] - 2022-03-29
## Added
- DTTAsset attribute 
- DTTAssetService class to automatically create dtt assets in the project.

## [3.0.0] - 2022-03-28
### Added
- DTTPathUtility class containing methods from runtime utilities PathUtility class

### Updated
- Dependency to runtime utilities to version 2.0.0

## [2.2.1] - 2022-03-18
### Fixed
- Set correct path for retrieving fonts.

## [2.2.0] - 2022-02-01
### Added
- New open sans and monsterrat fonts.
- DTT inspector for shaders.

## [2.1.0] - 2022-01-25
### Updated
- Updated dependecies to runtime utilities and editor utilities.

## [2.0.3] - 2022-01-13
### Updated
- Updated dependecies to runtime utilities and editor utilities.

## [2.0.2] - 2021-12-30
### Fixed
- Fixed issue with package/asset causing an infinite import loop at import or project load. 

## [2.0.1] - 2021-12-21
### Fixed
- Fixed problem with null reference exception when the text property of a readme section was non-existent.
- Fixed problem with null reference exception when the text property of a readme section didn't contain paragraphs
- Fixed an issue with the ContentFolderPath retrieval not returning a path that was unity compatible.

### Updated
- The ConfigPostProcessor now uses the serialized property of the asset json instead of writing to the file.
- The retrieval of asset json in the project now has a more descriptive warning message when failing.

## [2.0.0] - 2021-12-15
### Updated
- Updated editor utilities dependency to 3.0.0
- Updated runtime utilities dependency to 1.1.0

## [Initial Release]
## [1.0.0] - 2021-11-23
### Updated
- Updated editor utilities dependency to 2.0.0
- Updated runtime utilities dependency to 1.0.0

[Unreleased]

##[0.0.5] - 2021-08-23
### Fixed
- Prevent readme window from crashing whenever an image is not loaded correctly.

##[0.1.0] - 2021-08-25
### Added
- Added a new 'EditorTextures' class to publishing tools which uses the full package name to resolve a base path for a package making it possible to only need ot provide the relative path.

##[0.2.0] - 2021-08-25
### Removed
- Removed unnecessary method that caused an exception when removing another package.

### Added
- Added a property that provides all package paths of dtt packages in the project. This can be used when you want to look for assets in the project but want to reduce searches to only be inside of dtt package folders.

