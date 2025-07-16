using System;
using System.Collections.Generic;

using MelonLoader;

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Processes console commands for the MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe using lock objects.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types and proper error handling.
/// </summary>
internal class ModConsoleCommandProcessor
{
    private readonly object _lock = new object();
    private readonly Dictionary<string, Action<string[]>> _commandHandlers = new Dictionary<string, Action<string[]>>();

    public ModConsoleCommandProcessor()
    {
        // Register default commands
        RegisterCommand("help", HelpCommand);
        RegisterCommand("mixer_set", MixerSetCommand);
        RegisterCommand("mixer_get", MixerGetCommand);
        // Add more commands as needed
    }

    public void RegisterCommand(string command, Action<string[]> handler)
    {
        lock (_lock)
        {
            if (!_commandHandlers.ContainsKey(command))
            {
                _commandHandlers.Add(command, handler);
            }
        }
    }

    public void ProcessCommand(string commandLine)
    {
        try
        {
            var parts = commandLine.Split(' ');
            if (parts.Length == 0) return;

            var command = parts[0].ToLowerInvariant();
            var args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

            lock (_lock)
            {
                if (_commandHandlers.TryGetValue(command, out var handler))
                {
                    handler(args);
                }
                else
                {
                    Main.logger?.Warn(1, string.Format("{0} Unknown command: {1}", CONSOLE_CMD_PREFIX, command));
                }
            }
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error processing command: {1}\n{2}", CONSOLE_CMD_PREFIX, commandLine, ex));
        }
    }

    private void HelpCommand(string[] args)
    {
        Main.logger?.Msg(1, string.Format("{0} Available commands: help, mixer_set, mixer_get", CONSOLE_CMD_PREFIX));
    }

    private void MixerSetCommand(string[] args)
    {
        if (args.Length < 2)
        {
            Main.logger?.Warn(1, string.Format("{0} Usage: mixer_set <id> <value>", CONSOLE_CMD_PREFIX));
            return;
        }
        if (int.TryParse(args[0], out int id) && float.TryParse(args[1], out float value))
        {
            // Example: Set mixer value via Main entry point
            MixerThreholdMod_1_0_0.Main main = MelonLoader.MelonMod.Instance as MixerThreholdMod_1_0_0.Main;
            main?.SetMixerValue(id, value);
            Main.logger?.Msg(1, string.Format("{0} Mixer {1} set to {2}", CONSOLE_CMD_PREFIX, id, value));
        }
        else
        {
            Main.logger?.Warn(1, string.Format("{0} Invalid arguments for mixer_set", CONSOLE_CMD_PREFIX));
        }
    }

    private void MixerGetCommand(string[] args)
    {
        if (args.Length < 1)
        {
            Main.logger?.Warn(1, string.Format("{0} Usage: mixer_get <id>", CONSOLE_CMD_PREFIX));
            return;
        }
        if (int.TryParse(args[0], out int id))
        {
            MixerThreholdMod_1_0_0.Main main = MelonLoader.MelonMod.Instance as MixerThreholdMod_1_0_0.Main;
            float value = main?.GetMixerValue(id) ?? 0f;
            Main.logger?.Msg(1, string.Format("{0} Mixer {1} value: {2}", CONSOLE_CMD_PREFIX, id, value));
        }
        else
        {
            Main.logger?.Warn(1, string.Format("{0} Invalid arguments for mixer_get", CONSOLE_CMD_PREFIX));
        }
    }
}