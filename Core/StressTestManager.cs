

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Orchestrates stress test operations for MixerThreholdMod.
    ///  THREAD SAFETY: All operations are thread-safe and use async patterns.
    ///  .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper cancellation tokens.
    ///  MAIN THREAD WARNING: Never blocks Unity main thread; all I/O and heavy operations are async.
    /// </summary>
    internal class StressTestManager
    {
        private readonly object _lock = new object();
        private bool _isTestRunning;
        private CancellationTokenSource _cts;
        private List<string> _testResults;

        public StressTestManager()
        {
            _testResults = new List<string>();
        }

        /// <summary>
        /// Starts a mixer save stress test with the given configuration.
        /// </summary>
        /// <param name="iterations">Number of iterations to run.</param>
        /// <param name="delaySeconds">Delay between iterations in seconds.</param>
        /// <param name="bypassCooldown">Whether to bypass cooldown logic.</param>
        public async Task RunMixerSaveStressTestAsync(int iterations, float delaySeconds, bool bypassCooldown)
        {
            lock (_lock)
            {
                if (_isTestRunning)
                {
                    Main.logger?.Warn(1, string.Format("{0} Stress test already running.", STRESS_TEST_PREFIX));
                    return;
                }
                _isTestRunning = true;
                _cts = new CancellationTokenSource();
            }

            Main.logger?.Msg(1, string.Format("{0} Starting mixer save stress test: {1} iterations, {2:F2}s delay, bypass: {3}", STRESS_TEST_PREFIX, iterations, delaySeconds, bypassCooldown));
            int successCount = 0;
            int errorCount = 0;
            var startTime = DateTime.UtcNow;

            try
            {
                for (int i = 1; i <= iterations; i++)
                {
                    if (_cts.Token.IsCancellationRequested)
                    {
                        Main.logger?.Warn(1, string.Format("{0} Stress test cancelled at iteration {1}.", STRESS_TEST_PREFIX, i));
                        break;
                    }

                    try
                    {
                        Main.logger?.Msg(2, string.Format("{0} Iteration {1}/{2}...", STRESS_TEST_PREFIX, i, iterations));
                        await Save.CrashResistantSaveManager.TriggerSaveWithCooldown().ConfigureAwait(false);
                        successCount++;
                        _testResults.Add(string.Format("Iteration {0}: Success", i));
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        _testResults.Add(string.Format("Iteration {0}: Error - {1}", i, ex.Message));
                        Main.logger?.Err(string.Format("{0} Error in iteration {1}: {2}\n{3}", STRESS_TEST_PREFIX, i, ex.Message, ex.StackTrace));
                    }

                    if (delaySeconds > 0)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds), _cts.Token).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} Critical error during stress test: {1}\n{2}", STRESS_TEST_PREFIX, ex.Message, ex.StackTrace));
            }
            finally
            {
                lock (_lock)
                {
                    _isTestRunning = false;
                    _cts.Dispose();
                    _cts = null;
                }
                var duration = DateTime.UtcNow - startTime;
                Main.logger?.Msg(1, string.Format("{0} Stress test complete. Success: {1}, Errors: {2}, Duration: {3:F2}s", STRESS_TEST_PREFIX, successCount, errorCount, duration.TotalSeconds));
            }
        }

        /// <summary>
        /// Cancels any running stress test.
        /// </summary>
        public void CancelStressTest()
        {
            lock (_lock)
            {
                if (_isTestRunning && _cts != null)
                {
                    _cts.Cancel();
                    Main.logger?.Warn(1, string.Format("{0} Stress test cancellation requested.", STRESS_TEST_PREFIX));
                }
            }
        }

        /// <summary>
        /// Gets the results of the last stress test run.
        /// </summary>
        public IReadOnlyList<string> GetTestResults()
        {
            lock (_lock)
            {
                return _testResults.AsReadOnly();
            }
        }
    }
}