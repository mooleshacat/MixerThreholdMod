# GitHub Copilot Instructions for MixerThreholdMod

## Core Development Rules (Limit: 6)

1. **🚨 PRIORITY: Recent PR Comments**: Always prioritize and follow the most recent PR comment instructions unless explicitly overridden. Recent commit context takes precedence over historical instructions.

2. **🎯 .NET 4.8.1 Compatibility**: Use IL2CPP compatible & .NET 4.8.1 syntax. Use `string.Format()` over interpolation, explicit types, `default(CancellationToken)`, never yield return in try/catch blocks.

3. **🛡️ Thread Safety & Performance**: All operations thread-safe, never block Unity main thread. Use async/await with `ConfigureAwait(false)`, proper cancellation tokens, no Thread.Sleep.

4. **🚨 Extreme Verbose Debugging**: Comprehensive logging with detailed errors, stack traces, operation context. Try-catch all operations with specific error handling and recovery strategies.

5. **💾 Save Crash Prevention**: Prevent corruption during repeated saves/extended gameplay. Use atomic file operations, backup strategies, emergency fallback mechanisms.

6. **📝 Documentation & Workflow**: Include thread safety warnings, .NET 4.8.1 notes, main thread warnings in XML docs. Follow Git flow: feature/fix → development → master.

## Additional Context

- **Target Framework**: .NET Framework 4.8.1 (Unity/MelonLoader environment)
- **Primary Concern**: Preventing save crashes during repeated saves and extended gameplay sessions
- **Thread Model**: Unity main thread must never be blocked; prefer async patterns with proper synchronization
- **Error Handling**: Comprehensive exception handling with verbose logging; never let exceptions crash the game