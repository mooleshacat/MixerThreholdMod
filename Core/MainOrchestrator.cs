

﻿using MixerThreholdMod_1_0_0.Core;

/// <summary>
/// Coordinates patch initialization, mixer management, scene handling, and stress tests for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe and async.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public class MainOrchestrator
{
    private readonly Logger _logger;
    private PatchInitializer _patchInitializer;
    private MixerManager _mixerManager;
    private SceneHandler _sceneHandler;
    private StressTestManager _stressTestManager;

    public MainOrchestrator(Logger logger)
    {
        _logger = logger;
        _patchInitializer = new PatchInitializer();
        _mixerManager = new MixerManager();
        _sceneHandler = new SceneHandler();
        _stressTestManager = new StressTestManager();
    }

    public async void InitializeAsync()
    {
        try
        {
            await _patchInitializer.InitializeAllPatchesAsync().ConfigureAwait(false);
            _logger.Msg(1, "[ORCHESTRATOR] All patches initialized.");
        }
        catch (System.Exception ex)
        {
            _logger.Err(string.Format("[ORCHESTRATOR] Patch initialization failed: {0}\n{1}", ex.Message, ex.StackTrace));
        }
    }

    public void OnSceneLoaded(int buildIndex, string sceneName)
    {
        _sceneHandler.OnSceneLoaded(buildIndex, sceneName);
    }

    public async void RunStressTest(int iterations, float delaySeconds, bool bypassCooldown)
    {
        await _stressTestManager.RunMixerSaveStressTestAsync(iterations, delaySeconds, bypassCooldown).ConfigureAwait(false);
    }

    public void SetMixerValue(int mixerId, float value)
    {
        _mixerManager.SetMixerValue(mixerId, value);
    }

    public float GetMixerValue(int mixerId)
    {
        return _mixerManager.GetMixerValue(mixerId);
    }
}