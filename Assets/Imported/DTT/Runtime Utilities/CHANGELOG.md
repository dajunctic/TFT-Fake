# Changelog

- All notable changes to this package will be documented in this file.
- The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) and this package adheres to [Semantic Versioning](https://semver.org/)


## [2.0.3] - 2022-06-24

## Fixed
- Issue with Vector3Extensions.Flatten method causing build issues on iOS

## [2.0.2] - 2022-05-23
### Updated
- Updated minimum unity version
### Removed
- Removed changelog from documentation.

## [2.0.1] - 2022-04-07

### Updated
 - Package.json unity version set to 2019.4 and corect documentation linked.

## [2.0.0] - 2022-03-24

### Added
- ColorExtensions class
  - ToHex
- UIntExtensions class
  - ToColor
- Mathd class
- StringUtility class methods
  - IsWebUrl
  - IsVariableName
  - IsEmail
  - IsHexadecimal
- Vector3Extensions class
  - Flatten
- EnumExtensions method
  - GetInspectorName
- TransformExtensions class
  - FirstChild
  - LastChild
- EnumDropdown<T> class
- ListExtensions methods
  - RotateRight
  - RotateLeft
- GridBase<T> class

### Removed
- PathUtility methods
  - InDTTDirectory
  - IsAssetJson

## [1.1.3] - 2022-01-25
### Added
- Example scene with example scripts.

## [1.1.2] - 2022-01-13
### Removed
- Invalid Unit test.

## [1.1.1] - 2021-12-27
### Fixed
- Fixed missing version define and added TEST_FRAMEWORK defines to tests to prevent compile errors for VSCode users.

## [1.1.0] - 2021-11-29
### Added
- Add ToChar, ToInt, ToByte extension methods to EnumExtensions
- Add RandomInsecure method to new StringUtility class.
- Add Tests for new features.

## [Initial Release]

## [1.0.0] - 2021-11-22
### Added
- Add rect transform extensions.
- Add new StripHtmlTags method to StringExtensions. 

### Updated
- Update serializable dictionary and add serializable interface.
- Update serializable interface and dictionary to be unity 2020 only

## [Unreleased]

## [0.1.1] - 2021-11-15
### Fixed
 - Ellipsis method now properly cuts string and appends ellipsis characters.
 
### Added
 - Additional OnGUI tests for the Ellipsis method.