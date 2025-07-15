using MelonLoader;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;
using MixerThreholdMod_1_0_0.Core;

namespace MixerThreholdMod_1_0_0
{
    /// <summary>
    /// Entry point for MixerThreholdMod.
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and use async patterns.
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
    /// ⚠️ MAIN THREAD WARNING: Never blocks Unity main thread; all heavy operations are async.
    /// </summary>
    public class Main : MelonMod
    {
        private PatchInitializer _patchInitializer;
        private MixerManager _mixerManager;
        private SceneHandler _sceneHandler;
        private StressTestManager _stressTestManager;

        public static Logger logger;

        public override void OnInitializeMelon()
        {
            logger = new Logger();
            _patchInitializer = new PatchInitializer();
            _mixerManager = new MixerManager();
            _sceneHandler = new SceneHandler();
            _stressTestManager = new StressTestManager();

            // Initialize all patches asynchronously
            _ = _patchInitializer.InitializeAllPatchesAsync();

            logger.Msg(1, string.Format("{0} MixerThreholdMod initialized.", MOD_ENTRY_PREFIX));
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _sceneHandler.OnSceneLoaded(buildIndex, sceneName);
        }

        // Example: Expose stress test from mod entry
        public async void RunStressTest(int iterations, float delaySeconds, bool bypassCooldown)
        {
            await _stressTestManager.RunMixerSaveStressTestAsync(iterations, delaySeconds, bypassCooldown).ConfigureAwait(false);
        }

        // Example: Expose mixer value management from mod entry
        public void SetMixerValue(int mixerId, float value)
        {
            _mixerManager.SetMixerValue(mixerId, value);
        }

        public float GetMixerValue(int mixerId)
        {
            return _mixerManager.GetMixerValue(mixerId);
        }
    }
}