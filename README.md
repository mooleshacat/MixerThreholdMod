# MixerThreholdMod v1.0.0 🧪 - MONO/IL2CPP Compatible

A comprehensive mod for **Schedule 1** that enhances the mixer system with crash-resistant saves, increased capacity, and intelligent threshold control. **Now fully compatible with both MONO and IL2CPP builds!**

## Table of Contents
- [Features](#features)
- [Installation](#installation)
- [Console Commands](#console-commands)
- [Configuration](#configuration)
- [Technical Details](#technical-details)
- [Compatibility](#compatibility)
- [Developer Notes](#developer-notes)

## Features

### 🛡️ Crash Prevention & Save Management
- **Crash-Resistant Save System**: Atomic file operations with backup/recovery mechanisms
- **Emergency Save Functionality**: Automatic emergency saves before crashes or shutdowns
- **Save Cooldown System**: Prevents rapid-fire saves that can cause corruption
- **Backup Management**: Automatic backup creation with intelligent cleanup (5-minute intervals)
- **Save Path Detection**: Intelligent game directory detection with multiple fallback methods

### ⚡ Performance & Monitoring
- **Advanced System Performance Monitor**: Memory leak detection and performance baseline tracking
- **File Operation Diagnostics**: Comprehensive file I/O timing and transfer rate monitoring
- **System Resource Monitoring**: Real-time tracking of GC, process memory, and CPU usage
- **Thread-Safe Operations**: All operations designed to never block Unity's main thread
- **Cancellable I/O Runner**: Background I/O operations with proper cancellation support

### 🎚️ Mixer System Enhancements
- **Dynamic Threshold Control**: Real-time mixer threshold adjustments with value change detection
- **Event Listener Management**: Robust event attachment with multiple fallback strategies
- **Mixer Configuration Tracking**: Persistent mixer settings across game sessions
- **Polling Fallback System**: Emergency polling when direct event attachment fails
- **Mixer ID Management**: Comprehensive mixer identification and tracking system

### 🎮 Console Integration
- **Comprehensive Console Commands**: Full suite of debugging and management commands
- **Game Console Bridge**: Integration with Schedule 1's native console system
- **Command Processing**: Detailed command parsing with parameter validation
- **Logging System**: Multi-level logging with crash prevention and verbose diagnostics
- **Interactive Command Help**: Built-in help system for all available commands

### 🔧 Developer & Debug Tools
- **IL2CPP Type Resolver**: Safe type loading for IL2CPP compatibility
- **Game Exception Monitor**: Harmony patches for game console error monitoring
- **Safe File Locking System**: Thread-safe file operations with retry logic
- **Directory Resolver**: Game installation directory detection across multiple methods
- **Stress Testing Tools**: Built-in save and preference stress testing capabilities

### 🏗️ Architecture & Compatibility
- **MONO/IL2CPP Dual Compatibility**: Works seamlessly with both runtime environments
- **.NET Framework 4.8.1 Compatible**: Uses framework-appropriate patterns and syntax
- **Thread Safety First**: All operations designed with thread safety as primary concern
- **Constants Management**: 400+ centralized constants eliminating magic numbers
- **Harmony Integration**: Safe runtime patching with comprehensive error handling

## Console Commands

| Command | Description | Parameters |
|---------|-------------|------------|
| `help` | Display available commands and usage information | None |
| `mixer_reset` | Reset all mixer threshold values | None |
| `mixer_save` | Force save current mixer configuration | `[delay]` `[bypass]` |
| `mixer_path` | Display current save path information | None |
| `mixer_emergency` | Trigger emergency save | None |
| `savegamestress` | Run game save stress test | `<iterations>` `[delay]` |
| `saveprefstress` | Run mixer preferences stress test | `<iterations>` `[delay]` |
| `savemonitor` | Monitor save operations in real-time | None |

## Configuration

The mod automatically detects and configures itself based on the game environment. Key configuration includes:

- **Save File Location**: `MixerThresholdSave.json` in game's save directory
- **Emergency Backup**: `MixerThresholdSave_Emergency.json` in user data directory
- **Backup Rotation**: Automatic cleanup keeping 5 most recent backups
- **Save Cooldown**: 2-second cooldown between saves for stability
- **Performance Thresholds**: 100ms moderate, 500ms slow operation warnings

## Technical Details

### Thread Safety Architecture
- All file operations use background threads with `ConfigureAwait(false)`
- Proper locking mechanisms prevent save corruption
- Main Unity thread never blocked by I/O operations
- Cancellation token support throughout async operations

### Crash Prevention Mechanisms
- Atomic file operations (temp file + rename pattern)
- Emergency save hooks on crashes and shutdowns
- Extensive error handling with graceful degradation
- Multiple fallback strategies for critical operations

### IL2CPP Compatibility Features
- Minimal reflection usage with compile-time safe patterns
- No dynamic code generation or runtime type creation
- AOT-safe method resolution using `typeof()` instead of `GetType()`
- Interface-based processing instead of reflection-heavy approaches

## Compatibility

- **Unity Version**: Compatible with Schedule 1's Unity version
- **MelonLoader**: Full MelonLoader mod framework support
- **Runtime**: MONO and IL2CPP builds supported
- **.NET Framework**: 4.8.1 compatible syntax and patterns
- **Game Version**: Designed for Schedule 1 (maintains compatibility with game updates)

## Developer Notes

Yes I am playing with Github Copilot :)

"your mod name has a typo" - no actually it does not. I've reverse engineered using dnSpy Schedule 1 and I know the name was spelled with a typo in the main code. So while it is a typo, it is not a typo on my part ;)

### Code Organization
- **Core/**: Main system components and monitors
- **Helpers/**: Utility classes and helper functions  
- **Save/**: Save management and crash prevention
- **Patches/**: Harmony patches for game integration
- **Constants/**: Centralized constants (400+ constants)
- **Threading/**: Thread-safe operations and async utilities

### Key Technical Decisions
- **String.Format over String Interpolation**: Maximum .NET 4.8.1 compatibility
- **Explicit Type Declarations**: IL2CPP AOT compilation safety
- **Comprehensive Error Handling**: Prevents crashes during extended gameplay
- **Background Threading**: Never blocks Unity main thread
- **Constants Consolidation**: Eliminates magic numbers for maintainability