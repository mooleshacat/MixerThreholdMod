# MixerThreholdMod 🧪

A comprehensive mod for **Schedule 1** that enhances the mixer system with increased capacity, intelligent threshold control, and automatic save backup protection.

## About Schedule 1 🏭

Schedule 1 is a business simulation game where you run a drug manufacturing operation. One of the core mechanics involves mixing products (cannabis, meth, cocaine, etc.) with additives using the in-game mixer system. The mixer has a threshold setting that controls how many items the chemist waits for before starting the mixing process.

## What This Mod Does 🚀

### 🔧 Enhanced Mixer Capacity
- **Increases maximum mixer threshold from 10 to 20** - matching the game's maximum stack size
- **Persistent threshold settings** - your preferred settings won't reset when the game overwrites them
- **Smart threshold management** - prevents the game from forcing the threshold back to 1

### 💾 Automatic Save Backup System
- **Automatic backup of your last 5 saves** after each save operation
- **Crash protection** - if the mod (or any other mod) causes issues, your saves are safe
- **Timestamped backups** with easy-to-understand naming convention
- **Automatic cleanup** - old backups are removed to save disk space

### 🛡️ Stability & Safety Features
- **Comprehensive crash prevention** with extensive null checks and error handling
- **Memory leak protection** through proper resource disposal
- **Async operation safety** to prevent game freezing
- **Debug logging** for troubleshooting and monitoring

## Why This Mod is Essential 📈

**For Efficiency**: The default threshold of 10 forces you to wait longer for mixing operations. With items that stack to 20, you're losing potential efficiency. This mod lets you maximize your mixing operations.

**For Safety**: Extended gameplay sessions can lead to various stability issues. The automatic backup system ensures you never lose hours of progress due to crashes or corruption.

**For Control**: Take full control of your mixer settings without worrying about the game resetting your preferences.

## Installation 📥

1. Download the latest release from [Nexus Mods](https://www.nexusmods.com/schedule1) (when available)
2. Extract the mod files to your Schedule 1 Mods directory
3. Launch the game - the mod will automatically initialize

## Backup System 🗃️

### How It Works
After each save operation, the mod automatically:
1. Creates a timestamped copy of your save folder
2. Stores it in `MixerThresholdMod_backup` directory (next to your save folder)
3. Keeps the 5 most recent backups and removes older ones

### Backup Location
Your saves are stored every time you save game in:
```
C:\Users\YourName\AppData\LocalLow\TVGS\Schedule 1\Saves\nnnnnnnnnnnnnnnnn\
```
Where each 'n' is a number. Structure is like so:
- nnnnnnnnnnnnnnnnn\
  - SaveGame_1\
  - SaveGame_2\
  - SaveGame_3\
  - SaveGame_4\
  - SaveGame_5\
Your backups will be in:
```
C:\Users\YourName\AppData\LocalLow\TVGS\Schedule 1\Saves\nnnnnnnnnnnnnnnnn\MixerThreholdMod_backup\
```
Structure is like so:
- MixerThreholdMod_backup
  - SaveGame_2_backup_2025-07-10_18-22-48
  - SaveGame_2_backup_2025-07-10_18-26-12
  - SaveGame_2_backup_2025-07-10_18-29-16
  - SaveGame_2_backup_2025-07-10_18-36-51
  - SaveGame_2_backup_2025-07-10_18-50-19

There will be 5 backups for each savegame (5x5=25 max saves)

### Restoring from Backup 🔄

If you need to restore a backup:

1. **Locate your backup folder** (see location above)
2. **Find the backup you want to restore** - they're named with timestamps like:
   ```
   SaveGame_2_backup_2024-01-15_14-30-22
   ```
3. **Navigate to your main save directory** (usually `C:\Users\YourName\AppData\LocalLow\TVGS\Schedule 1\Saves\nnnnnnnnnnnnnnnnn\`)
4. **Backup your current save** (copy SaveGame_2 somewhere safe)
5. **Delete or rename SaveGame_2 folder**
6. **Copy the backup folder** to the save location
7. **Rename it** to `SaveGame_2` (or whatever your original save folder was named)

> ⚠️ **Important**: Always backup your current save before restoring, just in case!

## Features in Detail ⚙️

### Mixer Threshold Control
- Set thresholds between 1-20 (was previously limited to 1-10)
- Settings persist across game sessions and save/load cycles
- Real-time threshold adjustment through in-game UI
- Automatic enforcement prevents game from overriding your settings

### Advanced Error Handling
- Graceful handling of file system errors during backup operations
- Protection against infinite loops and memory leaks
- Comprehensive logging for troubleshooting
- Safe async operations that won't block the game

### Memory & Performance Optimization
- Proper disposal of file handles and resources
- Optimized backup cleanup with safety limits
- Non-blocking coroutines for smooth gameplay
- Minimal performance impact on save operations

## Compatibility 🔧

- **Game Version**: Schedule 1 (latest version)
- **Mod Framework**: MelonLoader
- **Dependencies**: HarmonyLib (included with MelonLoader)
- **Conflicts**: Should be compatible with most other mods

## Troubleshooting 🔍

### Common Issues

**Mod not loading**: Ensure MelonLoader is properly installed and the mod DLL is in the correct Mods folder.

**Backup not working**: Check that you have write permissions to your save game directory.

**Threshold resets**: The mod includes protection against this, but if it persists, check the debug logs for errors.

### Debug Information
The mod provides extensive logging. Check your MelonLoader console for:
- Backup operation status
- Threshold setting changes  
- Any errors or warnings

## Development Philosophy, Logging Approach, & Responsible AI Use 🤖

MixerThreholdMod is built with a focus on reliability, user experience, and maintainability. Throughout development, we made several intentional design choices to address common modding challenges and provide a robust experience for both users and developers:

- **Custom Logger for Configurable Verbosity:**  
  We implemented a custom `Logger` class instead of using the default MelonLoader logging. This allows for fine-grained control over log output, letting users and developers adjust verbosity levels dynamically. For example, detailed logs can be enabled during debugging, while release builds can suppress non-critical messages to avoid log spam. All log-level logic is centralized in the logger, so code remains clean and maintainable.

- **Defensive Programming & Error Handling:**  
  The mod uses extensive try/catch blocks and a global unhandled exception handler to ensure that unexpected errors are logged and do not crash the game. This is especially important in a modding context, where interactions with game code and other mods can introduce unpredictable behavior.

- **Async & Non-Blocking Operations:**  
  File operations and save/load routines are implemented using async patterns and coroutines. This ensures that disk IO and other potentially slow operations do not block the main game thread, maintaining smooth gameplay and responsiveness.

- **User-Centric Features:**  
  We enforce user settings (such as mixer thresholds) even when the base game attempts to override them. This guarantees that user preferences are respected, improving the overall experience.

- **Responsible Use of AI in Development:**  
  AI tools (such as GitHub Copilot and Claude Sonnet) have been used to assist with code generation, refactoring, and troubleshooting. However, all AI-generated code and suggestions are carefully reviewed, tested, and validated by a human developer. Context and instructions are communicated clearly and iteratively, ensuring that both the AI and the developer are aligned throughout the process. The development workflow is a true collaboration, with human oversight guiding every decision and verifying that the final implementation meets project standards and goals.

- **Respect for Community & Collaboration:**  
  We recognize that developers have different styles and preferences. Our choices reflect the needs of this project and our commitment to maintainability, configurability, and user experience. We welcome constructive feedback and encourage open discussion to continually improve the mod.

## Credits 👏

- **Author**: mooleshacat
- **Development**: Created with GitHub Copilot & Brave Leo's assistance
- **Game**: Schedule 1 by TVGS
- **Framework**: MelonLoader community
- **Anthropic**: for Claude Sonnet 3.5 (Leo is based off this model)
- **Brave & Leo AI**: for help getting the original source together
- **Github CoPilot**: for doing far more than Leo ever could

## License 📄

Open source - see LICENSE.md for details.

## Contributing 🤝

Issues, suggestions, and pull requests are welcome! This mod is open source and community contributions are encouraged. AI is welcomed :)

---

*Note: The mod name "MixerThreholdMod" intentionally preserves the original typo found in the game's code through dnSpy reverse engineering.*
