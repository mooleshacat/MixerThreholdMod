# MixerThreholdMod v1.0.0 🧪

A comprehensive mod for **Schedule 1** that enhances the mixer system with crash-resistant saves, increased capacity, and intelligent threshold control.

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
