# GitHub Copilot Instructions for MixerThreholdMod

## Core Development Rules (Limit: 6)

1. **🎯 .NET 4.8.1 Compatibility**: Always use IL2CPP compatible & .NET 4.8.1 compatible syntax. Use `string.Format()` instead of string interpolation, explicit type declarations, and `default(CancellationToken)` instead of `default`. Never use yield return in try/catch blocks.

2. **🛡️ Thread Safety First**: All operations must be thread-safe. Never block Unity's main thread with synchronous file I/O or Thread.Sleep. Use async/await patterns with `ConfigureAwait(false)` and proper cancellation tokens.

3. **🚨 Extreme Verbose Debugging**: Include comprehensive logging with detailed error messages, stack traces, and operation context. Use try-catch blocks around all operations with specific error handling and recovery strategies.

4. **💾 Save Crash Prevention Focus**: All code must prevent save corruption and crashes during repeated saves or extended gameplay. Use atomic file operations, backup strategies, and emergency fallback mechanisms.

5. **📝 Class-Level Documentation**: Every class must include thread safety warnings, .NET 4.8.1 compatibility notes, and main thread blocking warnings in XML documentation comments.

6. **🔄 Workflow Compliance**: Work on feature/fix branches → merge to development → merge to master. Development and master branches are protected. Follow proper Git flow and ensure all changes are tested before merging.

## Additional Context

- **Target Framework**: .NET Framework 4.8.1 (Unity/MelonLoader environment)
- **Primary Concern**: Preventing save crashes during repeated saves and extended gameplay sessions
- **Thread Model**: Unity main thread must never be blocked; prefer async patterns with proper synchronization
- **Error Handling**: Comprehensive exception handling with verbose logging; never let exceptions crash the game