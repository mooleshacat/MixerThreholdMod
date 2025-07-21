

using MelonLoader;

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

ï»¿using MelonLoader;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;
using MixerThreholdMod_1_0_0.Core;

/// <summary>
/// Entry point for MixerThreholdMod.
/// âš ï¸ THREAD SAFETY: All operations are thread-safe and use async patterns.
/// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// âš ï¸ MAIN THREAD WARNING: Never blocks Unity main thread; all heavy operations are async.
/// </summary>
public class MainEntryPoint : MelonMod
{
    public static Logger logger;
    private MainOrchestrator _orchestrator;

    public override void OnInitializeMelon()
    {
        logger = new Logger();
        _orchestrator = new MainOrchestrator(logger);

        _orchestrator.InitializeAsync();

        logger.Msg(1, string.Format("{0} MixerThreholdMod initialized.", MOD_ENTRY_PREFIX));
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        _orchestrator.OnSceneLoaded(buildIndex, sceneName);
    }
}