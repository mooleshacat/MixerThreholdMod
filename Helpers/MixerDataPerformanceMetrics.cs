

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using System.Diagnostics;
using MixerThreholdMod_1_0_0.Core;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Tracks and logs performance metrics for mixer data operations.
    ///  THREAD SAFETY: All operations are thread-safe.
    ///  .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    ///  MAIN THREAD WARNING: Never blocks Unity main thread.
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
                logger.Err(string.Format("{0} Measure: operationName is null or empty.", PERFORMANCE_METRICS_PREFIX));
                return;
            }
            if (action == null)
            {
                logger.Err(string.Format("{0} Measure: action is null for {1}.", PERFORMANCE_METRICS_PREFIX, operationName));
                return;
            }

            Stopwatch stopwatch = null;
            try
            {
                stopwatch = Stopwatch.StartNew();
                action();
                stopwatch.Stop();
                logger.Msg(1, string.Format("{0} {1} completed in {2} ms.", PERFORMANCE_METRICS_PREFIX, operationName, stopwatch.ElapsedMilliseconds));
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
                logger.Err(string.Format("{0} LogMetric: metricName is null or empty.", PERFORMANCE_METRICS_PREFIX));
                return;
            }
            try
            {
                logger.Msg(1, string.Format("{0} Metric {1}: {2}", PERFORMANCE_METRICS_PREFIX, metricName, value));
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("LogMetric failed for {0}: {1}\nStack Trace: {2}", metricName, ex.Message, ex.StackTrace));
            }
        }
    }
}