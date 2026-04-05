# Changelog

- All notable changes to this package will be documented in this file.
- The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) and this package adheres to [Semantic Versioning](https://semver.org/)

## [3.2.1] - 2022-05-23
### Updated
- Updated depenedency.

## [3.2.0] - 2022-03-23
### Added
- New overloads for GUIDrawTools and GUIDrawing
- HandleUtility class
- "Opened" boolean constructor argument for AnimatedToggleFoldout class

### Fixed
- Issue with ExtendedDropdownBuilder not properly adding indents when using AddItems instead of AddItem

## [3.1.0] - 2022-01-25
### Updated
- Updated dependecy to runtime utilities.

### Added
- Added new utilities like IsScenePartOfBuildSettings and GetOrCreateScriptableObject.
- Added example scene and scripts.

## [3.0.1] - 2022-01-13
### Updated
- Updated dependecy to runtime utilities.

## [3.0.0] - 2021-11-29
### Removed
- Removed inspector attributes
- Removed serializable types
- Update documentation link in asset.json file.

## [2.1.0] - 2021-11-29
### Fixed
- Update exposed editor and nested inspector editor to not cause unnecessary exceptions in some edge cases

### Added
- Add Contains, Remove, Add methods for serialized property array through serialized property extensions
- Add draft feature to serializable dictionary to make it possible to add classes as keys.
- Add SaveAssetsDelayed method to prevent null reference errors during AssetDatabase.SaveAssets calls when starting playmode.
- AssetUtility class
  - Provides preprocesing application through right-clicking assets in the project view
  - Preprocess script files
  - Add scene assets to the build settings
- TreeViewUtility class

### Updated
- GetComponentInPrefab now correctly returns null and throws exceptions 
- ContextDropdown builder now has correct summaries and an option Rectangle argument.

## [2.0.1] - 2021-11-24
### Fixed
- Fixed issue with tests creating compile errors in Unity versions lower than 2020.

## [2.0.0] - 2021-11-19
### Updated
- Updated runtime utilities dependency to 1.0.0

## [1.1.0] - 2021-11-18#
### Added
- Return serializable dictionary from runtime utilities.
- Return serializable type from runtime utilities.
- Add serializable interface.

## [Initial Release]

## [1.0.1] - 2021-11-18
### Updated
- Moved attributes from editor folder to runtime folder to avoid build errors.

## [Unreleased]

## [0.13.2] - 2021-11-18
## Fixed
- Move decorators to runtime folder to avoid build errors. 
- fix issue with extended editor causing exceptions.

## [0.3.0] - 2021-08-31
### Updated
- Make generic implementation for decorators by creating a Decorator class and making all added decorators derive from it.
- Make generic implementation for decorator drawers by creating a DecoratorDrawer class and making all added decorator drawers derive from it.

### Added
- Add indent argument to AnimatedFoldout constructor which will indent the content drawn inside the foldout.
- Add ExtendedEditor class which is a custom editor for a MonoBehaviour. As this editor is a fallback one, it will only be used if no custom editor has already been defined for the behaviour.
- Add NestInspectorEditor which draws nested inspectors for all properties of a serialized object that are of the ‘object reference’ type.
- Add EditorUtilitySettings and EditorUtilitySettingsProvider which add a project setting to define whether the NestInspectorEditor should be used for all MonoBehaviour editors.

## [0.2.0] 2021-08-27
### Updated
- Updated EditorStringExtensions to StringExtensions and move it to the Runtime folder.

### Fixed
- Updated SpriteUtility to not throw any unnecessary NullReferences when working with Unity internal sprites.

### Added
- TagMenu attribute and drawer
- LayerMenu attribute and drawer
- SceneMenu attribute and drawer
- Label attribute and drawer
- ReadOnly attribute and drawer
- ProgressBar attribute and drawer
- FlexTextArea attribute and drawer
- ShowAssetPreview attribute and drawer
- NestInspector attribute and drawer

## [0.1.13] - 2021-07-05

### Fixed
- Fix issues with SerializableType throwing a null reference on creation
- Fix issue with SerializableType not resetting properly after its referenced type has been deleted.

### Added
- Add OperPrefab and OpenScript methods to AssetDatabaseUtility
- Add ExposeMethod attribute for exposing a method in the inspector to be run.
- Add ExposeInterface attribute for exposing an interface to be assigned in the inspector.

### Version 0.1.4 - 2021-06-28
- Removed
- All publishing Tool code is removed and added to a new package called Publishing Tools.
- Removed class constrained on TypeSafeCache<T> to make it possible to create struct implementations.

### Added
- Added ValueCache<T> class for providing the caching of value types.
- Added Texture2DCache and ColorCache classes.
- Added EditorSceneUtility class to provide utility for recognizing whether a gameobject or component is part of a prefab scene.
- Added GUIDrawTools and GUIDrawing classes to provide additional GUI drawing utility methods like LinkLabel and Separator
- Added GUIColors to provide editor colors not yet provided by Unity.
- Version 0.0.28 - 2021-05-14

### Updated
- The ConfigPostProcessor now adds a preprocessing directive in the pattern “packagename_ASSETSTORE_RELEASE” when a package is added to the project instead of to the packages folder.
- The DTTEditorConfig now supports multiple locations of the DTT folder in the project. 
- The DTTEditorConfig now better supports asset.json and package data retrieval with better file path usage. 
- The DTTHeader and DTTReadMe now use the documentation link provided by the asset.json its documentationUrl property.
- The DTTHeader and DTTReadMe now support local file paths as documentation links.

### Added
- SerializedPropertyCache and RelativePropertyCache now provide methods to update and apply changes to properties.

### Removed
- Test_ConfigPostProcessor caused directories to be created in the project causing dangerous project changes that could potentially cause exceptions/errors so it is removed.

## [0.0.19] - 2021-05-10
### Updated
- Updated DTTProjectFolder property to be flexible enough to support multiple DTT folder locations in the project.

## [0.0.17] - 2021-05-06
### Added
- RelativePropertyCache class. (use cases when you have a serialized property that has its own properties as well).
- AnimatedToggleFoldout class. (AnimatedFoldout but with a toggle)

### Updated
- ConfigPostProcessor now doesn't give warnings when trying to update the "assetStoreRelease" flag inside the Packages folder. It does give a warning when your asset store release flag is true but your package is inside the Packages folder.
- Minor accessibility fixes based on Connection Status Package experience

## [0.0.7] - 2021-04-29
### Added
- DTTColors class holding relevant colors.
- DTTGUI class providing new GUI functionalities and styles.
- DTTGUILayout class providing new GUI functionalities without usage of a Rect argument.
- DTTInspector class that used together with a DTTHeader attribute can draw a nice DTT banner at the top of your inspector.
- AssetJson class provided by the DTTEditorConfig class containing relevant information about the package as an asset.
- AnimatedFoldout class to be used on its own or through use of the EditorAnimationExtensions class. 
- AssetDatabaseUtility class providing easy to use AssetDatabase operations.
- AutomatedEditor class providing a simple and fast creation of a custom editor.
- GUIContentCache and GUIStyleCache classes to provide a type-safe way to store and use your GUIContent and GUIStyle objects in your editor drawing.
- SerializedPropertyCache class to provide a type-safe way to retrieve and use serialized fields from your MonoBehaviours/ScriptableObjects in your editor scripts.
- EditorStringExtensions class to provide tools for making strings more display friendly. 
- ConstructableCache class to provide inherited classes like GUIStyleCache and GUIContentCache with a type-safe way to store data with a one time initialization mechanism.
- TypeSafeCache class to provide inherited classes like SerializedPropertyCache with a type-safe way to store and manage data.