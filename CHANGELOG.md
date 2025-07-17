# Changelog

All notable changes to MixerThreholdMod will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - Unreleased

### ✨ Added

- CRITICAL RECOVERY PHASE 6A-6C COMPLETE: Fixed TrackedMixers references, removed duplicate Console, added missing constant, initialized patches ([``5ce8ea7``](../../commit/5ce8ea77beea5e9167dd36ad5109aa8e3987ca9f))
- CRITICAL RECOVERY COMPLETE: Fixed remaining constant replacements and enhanced CancellableIoRunner - DIRECTORY_RESOLVER_PREFIX in GameDirectoryResolver.cs (64 replacements), PERSISTENCE_PREFIX in MixerDataPersistenceManager.cs (70 replacements), enhanced CancellableIoRunner.cs with IO_RUNNER_PREFIX, priority queue system, and progress reporting interface ([``3427706``](../../commit/34277069e62d9b257c4281b89846b2e89a373dcf))
- FINAL RECOVERY: Fixed remaining TrackedMixers references in log messages for semantic consistency ([``f1a75d1``](../../commit/f1a75d1f32759ce8ccf055afb6ac64908392a16e))
- Fix mixer threshold detection and namespace references for proper 20f maximum ([``d2b4d6a``](../../commit/d2b4d6aac3e7aa5ac79e576516683fbb6fd90f30))
- Enhanced mixer detection with comprehensive debugging and fallback scanning ([``32c900b``](../../commit/32c900b20d36015ef237caa38a1b723836f72252))
- Enhanced mixer detection with comprehensive debugging and fallback scanning ([``6c7fdd4``](../../commit/6c7fdd4029839a46c1e0211799adb5bcfa1df342))
- Fix compilation errors: TrackedMixers methods, HarmonyInstance access, and .NET 4.8.1 yield return compliance ([``7fcdba7``](../../commit/7fcdba72483eb466a43e7fdcf79cc98f59f48968))
- Fix compilation errors: TrackedMixers methods, HarmonyInstance access, and .NET 4.8.1 yield return compliance ([``fe5c412``](../../commit/fe5c412882f707c02dda5d8549c37dcf4741e94e))
- Enhanced mixer detection with comprehensive debugging and fallback scanning ([``4d96fe8``](../../commit/4d96fe86e5575d7fd4a3804912de06a449929d0e))
- Enhanced mixer detection with comprehensive debugging and fallback scanning ([``6c5aadd``](../../commit/6c5aadd963cd5fb65989c3ef49ec94ede02027ab))
- Fix compilation errors: TrackedMixers methods, HarmonyInstance access, and .NET 4.8.1 yield return compliance ([``f725166``](../../commit/f725166e9404a193260446ce4bd7a49c0084979b))
- Fix compilation errors: TrackedMixers methods, HarmonyInstance access, and .NET 4.8.1 yield return compliance ([``b4bca84``](../../commit/b4bca8457041f889074d79652f878cd44711a16d))
- Fix mixer detection and logger issues: remove problematic FindObjectsOfType scanner, fix logger logic, add modification warnings ([``5b432c0``](../../commit/5b432c0af595806b7fef02e5febfcddb392f033c))
- Fix mixer threshold detection and namespace references for proper 20f maximum ([``fd39caf``](../../commit/fd39caf61533ebed9adf2ccaec9e0789e5f2d1e9))

### ⚡ Improved

- PHASE 1 MAJOR EXPANSION: Added 600+ new constants - reflection, system, performance, UI, validation, Unity, IL2CPP patterns ([``b5b64e7``](../../commit/b5b64e7822778c6e776bbfda489c8b30d2523999))
- PHASE 2-4 COMPREHENSIVE COMPLETION: Enhanced 12 core files with semantic constants, thread safety validation, and systematic standards enforcement ([``0398372``](../../commit/0398372ef2d824d3a20b99b514f7e65d826a0c67))
- PHASE 1-3 COMPLETE: File integrity, constants, thread safety audit ([``aad0d2d``](../../commit/aad0d2d666270ccfb83a765e6e743e909ea54eb6))
- PHASE 7-8: Fix string interpolation for .NET 4.8.1 compatibility, thread safety audit ([``cbbb536``](../../commit/cbbb536ab24d4b2010d4a810fe7ccee5106734db))
- CRITICAL RECOVERY: Regenerated corrupted EntityConfiguration_Destroy_Patch.cs - fixed structural corruption, duplicate code blocks, syntax errors ([``61793d6``](../../commit/61793d64124c767e1f4e0094b62debf32aceaff7))
- CRITICAL RECOVERY TASK 3 COMPLETE: Fixed SafeFileLockingSystem.cs structural corruption - removed duplicate class definitions, fixed logging, replaced hardcoded strings with FILE_LOCK_PREFIX constant ([``b5aa5c5``](../../commit/b5aa5c5df61bff310957596599b1dcb58ee68ad9))
- CRITICAL RECOVERY TASKS 4-11 COMPLETE: Fixed constants usage, created PerformanceOptimizer.cs, added ConfigureAwait(false), initialized missing patches ([``a30c29c``](../../commit/a30c29c8ff498981a63f915fe179cfc8cb5a4223))
- CRITICAL RECOVERY: Added missing constants to Constants.cs - Performance optimizer, logging prefixes, file operation constants ([``67ce563``](../../commit/67ce563f36ddcab6339fe462db22e6de4884db3e))
- Performance optimization, remove logs, csproj update ([``43d1c9a``](../../commit/43d1c9a273d1c2ca06d9214cfad6ee2ab6fefcac))
- File structure refactoring with descriptive names and enhanced IL2CPP memory monitoring ([``ae27081``](../../commit/ae27081d92d8d4d5dcdcda40d96d2c3e87837775))
- File structure refactoring with descriptive names and enhanced IL2CPP memory monitoring ([``8f7b00f``](../../commit/8f7b00f054d31053567533b24a4a5494b64f923f))
- Performance optimizations, cache manager ([``2d56682``](../../commit/2d5668226c82cb595b3618eff254182582d5f82c))
- Performance optimizations, cache manager ([``0b3403d``](../../commit/0b3403d7b8ab52d7603e68897e9c2597d5a78b5b))
- Resolved many compile errors, and one runtime exception. fixed "performance optimization manager" ([``eba8664``](../../commit/eba866479ec7bb6c042c4d6c7e0a1622e8fc0d3b))
- Resolved many compile errors, and one runtime exception. fixed "performance optimization manager" ([``2e3d016``](../../commit/2e3d01638346af25830beceb53325cf5ebbd8fc0))
- ... and 19 more changes

### 🐛 Fixed

- Comprehensive Constants.cs consolidation and code reorganizationFeatures:- Centralized 400+ constants from across the entire codebase- Organized constants by functionality (logging, performance, file operations, etc.)- Comprehensive documentation for every constant- IL2CPP compatible - compile-time constants safe for AOT- Thread-safe - all constants are immutable- .NET 4.8.1 compatible - explicit const declarationsCode Organization:- Constants/ - Centralized constants management- Core/ - Core functionality and managers- Helpers/ - Utility and helper classes  - Save/ - Save system management- Patches/ - Harmony patchesThis recreates and enhances the comprehensive work from PR #13 that wasinadvertently lost during the commit signing process. All functionalityand improvements are preserved and enhanced.Fixes: #13 (recreated and enhanced) ([``8d366af``](../../commit/8d366af75786f4f5e0dfe7383c72015a08997ad9))
- Fix savegamestress command and enhance documentation ([``d98302b``](../../commit/d98302bcaca66e1b4bf04958fa715bfff5a6f59f))
- Fix savegamestress command and enhance documentation ([``ce13984``](../../commit/ce13984ecec98c8a9b9ea5abaa891769cbfcc1cb))
- Save crash investigation (#12) ([``4ccc394``](../../commit/4ccc394786753328875453d6831195525ca6e85a))
- Save crash investigation (#12) ([``c26aab7``](../../commit/c26aab70b8dd3571d3aec3d89bcada8394ec0be5))
- Fix coroutine crashes and improve async patterns to prevent game freezing during saves (#7) ([``9438084``](../../commit/9438084c77e59ae9c68e43273a029ff171fc53e7))
- Restore critical helper files: FileOperations, MixerSaveManager, Utils, FileLockerHelper, CancellableIoRunner ([``a288418``](../../commit/a288418079cee2202fde03822dd4aa9a414553ef))
- Fix coroutine crashes and improve async patterns to prevent game freezing during saves (#7) ([``22b5400``](../../commit/22b54005daaff7ddde7b11af8324b8c804e577b7))
- Staged dnSpy decompile, updated ForCopilot instruction file, added 2 logs - second latest log is crash on game load, but reloading game seemed to work after (the latest log can be compared to the other logs) ([``3100bfe``](../../commit/3100bfe7f1bb609eae53cd3db07c8811eebacb80))
- Restore critical helper files: FileOperations, MixerSaveManager, Utils, FileLockerHelper, CancellableIoRunner ([``f1592a8``](../../commit/f1592a8bd1f2cbaa7cbee5641cf7e80c520f8ec6))
- Implement comprehensive dnSpy integration features: multi-method save validation, enhanced permissions, transactional saves ([``1fb865e``](../../commit/1fb865ec4470979753ff954b65cc263af11d29d3))
- Implement comprehensive dnSpy integration features: multi-method save validation, enhanced permissions, transactional saves ([``e1cfe1a``](../../commit/e1cfe1a872b5e2a055d69915e8394e9c2b520932))
- Fix coroutine crashes and improve async patterns to prevent game freezing during saves (#7) ([``dc2fb27``](../../commit/dc2fb278866bcd5a5bee3474c153bbae76b36baa))
- Fix coroutine crashes and improve async patterns to prevent game freezing during saves (#7) ([``b62cdcd``](../../commit/b62cdcdc3eb1e832d82d29f7dd4d505a35d796af))
- Implement comprehensive dnSpy integration features: multi-method save validation, enhanced permissions, transactional saves ([``b80db0c``](../../commit/b80db0c67a1f27df327b938198c9c8b4a0f67718))
- ... and 12 more changes

### 🔄 Changed

- PHASE 1 CONSTANTS IMPLEMENTATION: Systematic replacement of hardcoded values with semantic constants across 8 key files ([``d9ccf77``](../../commit/d9ccf77484ab0041270f97bdfc7a1989779b25e6))
- PHASE 4 COMPLETE: Separation of concerns refactor and final optimizations ([``0e260b4``](../../commit/0e260b4fa94d595f1f3627ea32ab1fe35c7ab8d3))
- PHASES 1-4 COMPLETE: Comprehensive 4-phase refactor with 96.2% compliance - using statements audit, critical error detection, file structure validation, and automated standards enforcement ([``2dffbe0``](../../commit/2dffbe0748a8321718b0338e873839c89baf74f3))
- Add comprehensive constants refactor documentation and README ([``e878b5f``](../../commit/e878b5f58d38ab5560b8d9f1860b33aac9a8e6bf))
- Commit helper devops scripts ([``457d793``](../../commit/457d7938dd135bbd4030c2dff9ac5177c87700f1))
- Final commit of our starter DevOps suite to preserve syntactic perfection and architechtural soundness featuring separation of concerns and best practices :) ([``65c8c86``](../../commit/65c8c86ba41d7ea41458cf49571471e7bccffb55))
- PHASE 1-4 COMPLETE: Constants expansion to 832 constants, comprehensive organization, .NET 4.8.1 compatibility audit, and systematic hardcoded value replacement ([``d32489e``](../../commit/d32489e9d430645ab6c993a2cd09c16c651d0d11))
- Complete constants refactor - 1785 constants across 12 domain files ([``b9d508c``](../../commit/b9d508cf5b299daba30b67a64b83abc65692a0f8))
- PHASE 8 COMPLETED: MONO Compatibility - fixed all string interpolation for .NET 4.8.1 compatibility ([``85e502e``](../../commit/85e502eadd224b48d1d20e6428c8efdee70c7642))
- Commit for copilot recovery operations ([``5719c2c``](../../commit/5719c2c734c40fc3ceadf3b92bf925c3166ff0f2))
- Complete PHASE 1-5: Constants consolidation (400 constants), README.md refactor, codebase audit ([``78defac``](../../commit/78defac7245173bba01145a877fe8fe341e1eeeb))
- CRITICAL RECOVERY: Fixed catastrophic file corruption and missing critical components ([``9b75e49``](../../commit/9b75e492ed30fbb505b36b3335d92ba167e56e25))
- PHASE 2-3 COMPLETE: Constants consolidation and project structure optimization ([``929d423``](../../commit/929d423aaabbc247bfca7adc422fcd589a570abd))
- Update gitignore ([``5abc180``](../../commit/5abc18067fdf7cbcb04bc9dcac87abea51a1a2a0))
- Update gitignore ([``370a777``](../../commit/370a7773e5c8128b28e4d22b20ec59ef9f9037bf))
- ... and 95 more changes

### 🗑️ Removed

- Remove blasted directory ([``ab1d66e``](../../commit/ab1d66e9c4298cc8d8a829d892253517c87fa46e))
- Clean copilot instructions - remove merge conflicts and sensitive references ([``2bbcc94``](../../commit/2bbcc94c247e5cbb644a3bcb067752d194715566))
- Final cleanup: removed legacy files, cleaned project structure, completed comprehensive refactoring ([``882d120``](../../commit/882d12084ed1f455448ce8cbd2bf97e74fddcad4))
- Final cleanup: removed legacy files, cleaned project structure, completed comprehensive refactoring ([``c8fe5db``](../../commit/c8fe5db76b63f493f5d7654d74e15ce5232dc66d))
- Made suggested changes by Github copilot chat - please ensure it did not remove code, and I can confirm it truncated a few files output, and so I was unable to make changes. Please ensure the logger helper is properly named, in the proper class and functions across the entire codebase. Errors attached to PR. ([``a24b284``](../../commit/a24b2843a4fea183f18381cfd0771a40eb6c0cdf))
- Final cleanup: removed legacy files, cleaned project structure, completed comprehensive refactoring ([``365405f``](../../commit/365405f7c6fe2b973d86953500222a62da5e1f6f))
- Final cleanup: removed legacy files, cleaned project structure, completed comprehensive refactoring ([``fefcdb5``](../../commit/fefcdb5b1d9a73f69dec5a226d8bdb010e73a324))
- Made suggested changes by Github copilot chat - please ensure it did not remove code, and I can confirm it truncated a few files output, and so I was unable to make changes. Please ensure the logger helper is properly named, in the proper class and functions across the entire codebase. Errors attached to PR. ([``6d38561``](../../commit/6d38561886c58e7bc756e61736240210aaf23c6e))

---

## About This Changelog

- **Generated**: 2025-07-17 03:09:27
- **Total Commits**: 193
- **Version Groups**: 
- **Current Version**: 1.0.0

### Category Legend

- **⚠️ BREAKING CHANGES**: Changes that may break existing functionality
- **🔒 Security**: Security-related improvements and fixes
- **✨ Added**: New features and functionality
- **⚡ Improved**: Performance improvements and enhancements
- **🐛 Fixed**: Bug fixes and error corrections
- **🔄 Changed**: Changes to existing functionality
- **🗑️ Removed**: Removed features and deprecated functionality
- **📝 Documentation**: Documentation updates and improvements

_This changelog is automatically generated from git commit history._
