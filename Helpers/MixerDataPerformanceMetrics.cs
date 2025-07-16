using System;
using System.Diagnostics;
using MixerThreholdMod_1_0_0.Core;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Tracks and logs performance metrics for mixer data operations.
    /// ⚠️ THREAD SAFETY: All operations are thread-safe.
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// ⚠️ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class MixerDataPerformanceMetrics
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Measures and logs the time taken for a mixer data operation.
        /// </summary>
        /// <param name="operationName">Name of the operation being measured.</param>
        /// <param name="action">Action representing the operation.</param>
        public static void Measure(string operationName, Action action)
        {
            if (string.IsNullOrEmpty(operationName))
            {
                logger.Err("[MixerDataPerformanceMetrics] Measure: operationName is null or empty.");
                return;
            }
            if (action == null)
            {
                logger.Err(string.Format("[MixerDataPerformanceMetrics] Measure: action is null for {0}.", operationName));
                return;
            }

            Stopwatch stopwatch = null;
            try
            {
                stopwatch = Stopwatch.StartNew();
                action();
                stopwatch.Stop();
                logger.Msg(1, string.Format("[MixerDataPerformanceMetrics] {0} completed in {1} ms.", operationName, stopwatch.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("Measure failed for {0}: {1}\nStack Trace: {2}", operationName, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Logs a custom performance metric value.
        /// </summary>
        /// <param name="metricName">Name of the metric.</param>
        /// <param name="value">Value to log.</param>
        public static void LogMetric(string metricName, double value)
        {
            if (string.IsNullOrEmpty(metricName))
            {
                logger.Err("[MixerDataPerformanceMetrics] LogMetric: metricName is null or empty.");
                return;
            }
            try
            {
                logger.Msg(1, string.Format("[MixerDataPerformanceMetrics] Metric {0}: {1}", metricName, value));
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("LogMetric failed for {0}: {1}\nStack Trace: {2}", metricName, ex.Message, ex.StackTrace));
            }
        }
    }
}