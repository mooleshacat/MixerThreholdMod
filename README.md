# MixerThreholdMod v1.0.0 🧪 - MONO/IL2CPP Compatible

A comprehensive mod for **Schedule 1** that enhances the mixer system with crash-resistant saves, increased capacity, and intelligent threshold control. **Now fully compatible with both MONO and IL2CPP builds!**

## Build Environment Compatibility 🔄

- [Critical Features](#critical-features-🚀)
- [Installation](#installation-📥)
- [Console Commands](#console-commands-🎮)
- [Known Issues](#known-issues-⚠️) **⚠️ IMPORTANT** 
- [Troubleshooting](#troubleshooting-🛠️)
- [Technical Implementation](#technical-implementation-⚙️)
- [Development & Contributions](#development--contributions-👥)
- [Reporting Issues](#reporting-issues-🐛)

## Critical Features 🚀

### 🛡️ **Save Crash Prevention** (Primary Focus)
- **Emergency save protection** - prevents data loss during crashes and extended gameplay
- **Atomic file operations** - ensures save integrity through temp file + rename strategy  
- **Save cooldown system** - prevents corruption from rapid-fire saves
- **Automatic backup management** - maintains 5 most recent backups with cleanup

### 🔧 **Enhanced Mixer System**
- **Increases mixer threshold from 10 to 20** - matching game's maximum stack size
- **Persistent threshold settings** - survives game overwrites and save/load cycles
- **Thread-safe operations** - prevents UI freezes and main thread blocking

## Installation 📥

1. Download latest release from [Nexus Mods](https://www.nexusmods.com/schedule1) (when available)
2. Extract to your Schedule 1 Mods directory
3. Launch game - mod auto-initializes with comprehensive logging

## Console Commands 🎮

The mod provides 13 powerful console commands for testing, monitoring, debugging, and advanced save operations:

### Available Commands

#### **Mixer Management**

##### `mixer_reset`
Reset all mixer values and clear tracked data:
```
mixer_reset                     # Clears all saved mixer values and resets ID counter
```

##### `mixer_save` 
Force immediate save operation:
```
mixer_save                      # Triggers save with cooldown protection
```

##### `mixer_path`
Display current save path and mixer count:
```
mixer_path                      # Shows active save location and tracked mixer count
```

##### `mixer_emergency`
Trigger emergency save operation:
```
mixer_emergency                 # Immediate save without cooldown checks
```

#### **Advanced Save Testing**

##### `saveprefstress <count> [delay] [bypass]`
Stress test mixer preferences saves:

**Parameters:**
- `count` - Number of save iterations (required, positive integer)
- `delay` - Delay between saves in seconds (optional, default: 0)
- `bypass` - Cooldown behavior (optional, default: true)
  - `true` (bypass) - Ignores game's 2-second save cooldown for rapid testing
  - `false` (respect cooldown) - Waits for game's natural save cooldown, safer for production

```
saveprefstress 10               # 10 saves, no delay, bypass=true (rapid testing)
saveprefstress 5 2.0            # 5 saves, 2s delay, bypass=true
saveprefstress 20 false         # 20 saves, no delay, respect cooldown (safer)
saveprefstress 10 0.1 false     # 10 saves, 0.1s delay, respect cooldown
```

##### `savegamestress <count> [delay] [bypass]`
Stress test game saves (calls SaveManager directly):

**Parameters:**
- `count` - Number of save iterations (required, positive integer) 
- `delay` - Delay between saves in seconds (optional, default: 0)
- `bypass` - Cooldown behavior (optional, default: true)
  - `true` (bypass) - Ignores game's 2-second save cooldown for rapid testing
  - `false` (respect cooldown) - Waits for game's natural save cooldown, safer for production

```
savegamestress 10               # 10 saves, no delay, bypass=true (rapid testing)
savegamestress 5 3.0            # 5 saves, 3s delay, bypass=true
savegamestress 3 false          # 3 saves, no delay, respect cooldown (safer)
savegamestress 5 2.0 false      # 5 saves, 2s delay, respect cooldown
```

##### `savemonitor <count> [delay] [bypass]`
Comprehensive save monitoring with multi-method validation:

**Parameters:**
- `count` - Number of save iterations (required, positive integer)
- `delay` - Delay between saves in seconds (optional, default: 0)
- `bypass` - Cooldown behavior (optional, default: true)
  - `true` (bypass) - Ignores game's 2-second save cooldown for rapid testing
  - `false` (respect cooldown) - Waits for game's natural save cooldown, safer for production

```
savemonitor 5                   # 5 saves, no delay, bypass=true (rapid testing)
savemonitor 3 2.0               # 3 saves, 2s delay, bypass=true  
savemonitor 10 false            # 10 saves, no delay, respect cooldown (safer)
savemonitor 5 1.5 false         # 5 saves, 1.5s delay, respect cooldown
```

##### `transactionalsave`
Performs atomic transactional save operation with timing and error recovery:
```
transactionalsave               # Single save with comprehensive logging
```

##### `profile`
Advanced save operation profiling with memory tracking and phase breakdown:
```
profile                         # Full save profiling with detailed metrics
```

#### **Manual Logging**

##### `msg <message>`
Log info message with manual tag:
```
msg Testing mixer behavior at threshold 0.8
msg Save operation completed successfully
```

##### `warn <message>`
Log warning message with manual tag:
```
warn Performance degradation detected during stress test
warn Memory usage approaching critical levels
```

##### `err <message>`
Log error message with manual tag:
```
err Critical save failure - investigating corruption
err Mixer threshold validation failed
```

#### **Help and Information**

##### `help` or `?`
Display complete command reference with usage examples:
```
help                             # Show all available commands with examples
?                                # Alias for help command
```

**Command-Specific Help**: When you use a command without required parameters, detailed help is automatically displayed:
```
saveprefstress                   # Shows specific help for saveprefstress command
msg                              # Shows specific help for msg command
```

### Console Access

**In-Game Console**: Open with `F4` (MelonLoader console) or use the game's console system
**Command Format**: Type command directly (no prefix required)
**Help Command**: Type any unrecognized command to display the complete help menu
**Logging**: All commands provide detailed output and error reporting

### Command Categories

- **4 Mixer Management Commands** - Basic mixer operations and path info
- **5 Advanced Save Testing Commands** - Stress testing and monitoring with dnSpy integration  
- **3 Manual Logging Commands** - Direct logging for debugging and testing
- **1 Help Command** - Complete command reference and specific command help

### Command Features

- **Thread-safe execution** - All commands respect main thread safety
- **Comprehensive logging** - Detailed progress and error reporting  
- **Performance metrics** - Timing and memory usage tracking
- **System monitoring** - Hardware performance tracking (DEBUG mode only)
- **Error recovery** - Graceful handling of save failures
- **Cooldown management** - Bypass or respect save cooldown systems
- **Parameter flexibility** - Most commands support flexible parameter ordering
- **Usage validation** - Helpful error messages and examples for incorrect usage

### System Monitoring (DEBUG Mode Only) 🖥️

**Hardware Performance Tracking**: Enhanced debugging capabilities that activate only in DEBUG builds:

**Features**:
- **CPU Usage Monitoring** - Real-time processor utilization during save operations
- **Memory Usage Tracking** - RAM consumption with garbage collection analysis
- **Disk I/O Performance** - Storage operation monitoring and bottleneck detection
- **Process Metrics** - Working set, private memory, and CPU time tracking
- **Performance Profiling** - Before/after system state comparisons for operations

**System Information Logging**:
- Operating system and .NET framework details
- Processor specifications (cores, clock speed)
- Memory configuration (total RAM, graphics memory)
- Storage device information and free space
- Unity engine hardware detection

**Integration Points**:
- All console commands include system performance snapshots
- Stress testing operations log system state at key intervals
- Save operations monitored for performance impact
- Comprehensive performance summaries with overhead analysis

**Compiler Directives**: 
- Automatically disabled in RELEASE builds for production deployment
- No performance impact when not in DEBUG mode
- Graceful degradation when WMI or performance counters unavailable

## Save Backup System 🗃️

**Automatic Protection**: Every save operation creates timestamped backups in:
```
C:\Users\YourName\AppData\LocalLow\TVGS\Schedule 1\Saves\nnnnnnnnnnnnnnnnn\MixerThreholdMod_backup\
```

**Backup Structure**:
- Keeps 5 most recent backups per save slot (max 25 total)
- Format: `SaveGame_2_backup_2025-01-15_14-30-22`
- Automatic cleanup prevents disk space issues

**Restore Process**:
1. Navigate to backup folder above
2. Copy desired backup to main save directory
3. Rename to original save folder name (e.g., `SaveGame_2`)
4. ⚠️ **Always backup current save first!**

## Technical Details ⚙️

### Thread Safety & .NET 4.8.1 Compatibility
- **No main thread blocking** - all file I/O uses async patterns or proper timeout protection
- **Thread-safe collections** - concurrent access handled safely across all operations  
- **Framework compatibility** - explicit .NET 4.8.1 syntax (no string interpolation, proper cancellation tokens)
- **Comprehensive error handling** - extensive try/catch blocks with verbose logging

### Performance & Reliability
- **Memory leak prevention** - proper resource disposal and cleanup
- **Atomic operations** - temp file strategy prevents partial writes
- **Emergency fallbacks** - multiple strategies for event attachment and save operations
- **Minimal game impact** - non-blocking coroutines maintain smooth gameplay

## Compatibility & Requirements 🔧

- **Game**: Schedule 1 (latest version)
- **Framework**: MelonLoader + HarmonyLib
- **Target**: .NET Framework 4.8.1
- **Conflicts**: Compatible with most mods (extensive defensive programming)

## Known Issues ⚠️

### ⚠️ **Critical MelonLoader Window Warning** ⚠️

**DO NOT CLICK** in the MelonLoader console window to inspect logs while the game is running:

- **Clicking pauses MelonLoader** and can cause the game to crash after a brief delay
- **To resume**: Press `Space` or `Enter` key, but damage may already be done
- **Safe inspection**: Use `Alt + Tab` to switch between windows - do not click inside the MelonLoader window
- **Log review**: Review logs after closing the game to avoid interruption

## Troubleshooting 🛠️

### Common Issues

#### Console Commands Not Working
- **MONO builds**: Ensure you're typing commands correctly (case-sensitive)
- **IL2CPP builds**: Wait for complete mod initialization (look for "initialization COMPLETE" message)
- Try the `help` command to verify mod is loaded
- Check MelonLoader logs for initialization errors

#### IL2CPP Build Issues
- **TypeLoadException messages**: Normal during initialization - mod handles these automatically
- **Slow initialization**: IL2CPP builds need 5-10 seconds for type resolution
- **Console commands delayed**: First few commands may take longer to process
- **Manual logging**: Use `msg` and `warn` commands to verify console integration is working

#### Save System Issues  
- Use `mixer_path` to verify save location
- Check available disk space in save directory
- Review logs for file permission errors

#### Performance Issues
- Use `profile` command to identify bottlenecks
- Monitor system resources with stress testing commands
- Consider reducing stress testing parameters

### Getting Detailed Support

**When reporting issues**, please include:

1. **Complete MelonLoader logs** - Full console output from mod startup
2. **System specifications** - CPU, RAM, GPU, OS version
3. **Game version** - Schedule 1 version number  
4. **Reproduction steps** - Exact steps to reproduce the issue
5. **Console command logs** - Output from relevant console commands
6. **Save game details** - Size, location, backup status

**Issue Report Template**:
```
Environment: [Game version, OS, hardware specs]
Mod Version: v1.0.0
Issue: [Brief description]
Steps to Reproduce: [Numbered steps]
Expected vs Actual: [What should happen vs what happened] 
Logs: [Attach complete MelonLoader logs]
Console Commands Used: [Any commands that triggered the issue]
```

**Post issues to**: [Repository Issues](https://github.com/mooleshacat/MixerThreholdMod/issues)

## Issue Reporting & Support 🐛

**When reporting issues, please provide as much detail as possible to help with troubleshooting:**

### Required Information
1. **MelonLoader Console Log** - copy the entire console output from when the issue occurs
2. **Save Game Details** - mention if issue happens on new saves, loaded saves, or specific save slots
3. **Reproduction Steps** - exact steps that cause the issue
4. **Game State** - how long was game running, were you saving repeatedly, any other mods active
5. **System Info** - Windows version, game version, MelonLoader version
6. **Error Messages** - any error dialogs, crashes, or unusual behavior

### How to Get Logs
- **MelonLoader Console**: Shows in-game when pressing F4 or check `MelonLoader/Latest.log`
- **Mod Logs**: Look for entries starting with `[MixerThreholdMod]`
- **Crash Logs**: Usually in game directory or MelonLoader folder

### Best Practices for Reporting
- **Be Specific**: "Save crashes" is less helpful than "Save crashes after 30+ minutes gameplay on SaveGame_3"
- **Include Context**: Were you using other mods, doing specific actions, or experiencing performance issues?
- **Test Isolation**: If possible, try reproducing with only this mod active

**The more information you provide, the faster we can identify and fix the issue!**

## Development & Contributions 🤝

### Built With Reliability First
- **Crash Prevention Focus** - specifically addresses save corruption issues from extended gameplay
- **Thread Safety** - comprehensive protection against UI freezes and deadlocks
- **Defensive Programming** - extensive error handling and fallback mechanisms
- **Verbose Logging** - detailed debugging information for troubleshooting

### AI-Assisted Development  
MixerThreholdMod leverages AI tools (GitHub Copilot, Claude) for code generation and refactoring, with all output carefully reviewed and tested by human developers. The collaboration ensures both efficiency and quality.

### Contributing
Issues, suggestions, and pull requests welcome! This mod is open source and community contributions are encouraged.

**Development Workflow**: feature/fix branches → development → master (protected branches)

## Credits & License 👏

- **Author**: mooleshacat  
- **Framework**: MelonLoader community
- **AI Assistance**: GitHub Copilot, Brave Leo (Claude-based)
- **License**: Open source - see LICENSE.md

---

*Note: "MixerThreholdMod" preserves the original typo found in Schedule 1's code through reverse engineering.*
