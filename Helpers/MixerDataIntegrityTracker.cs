using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Tracks and verifies mixer data integrity using hashes and validation.
/// ⚠️ THREAD SAFETY: All operations are thread-safe.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class MixerDataIntegrityTracker
{
    private static readonly ConcurrentDictionary<string, string> _dataHashes = new ConcurrentDictionary<string, string>();
    private static readonly ConcurrentDictionary<string, DateTime> _lastValidationTimes = new ConcurrentDictionary<string, DateTime>();

    public static string ComputeDataHash(Dictionary<int, float> mixerData)
    {
        try
        {
            var sortedData = mixerData.OrderBy(kvp => kvp.Key).ToList();
            string dataString = string.Join("|", sortedData.Select(kvp => string.Format("{0}:{1:F6}", kvp.Key, kvp.Value)));

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dataString));
                return Convert.ToBase64String(hashBytes);
            }
        }
        catch (Exception ex)
        {
            Main.logger?.Warn(1, string.Format("{0} MixerDataIntegrityTracker: Hash error: {1}", PERSISTENCE_PREFIX, ex.Message));
            return string.Format("ERROR_{0}", DateTime.Now.Ticks);
        }
    }

    public static void TrackHash(string key, string hash)
    {
        _dataHashes.TryAdd(key, hash);
        _dataHashes.TryUpdate(key, hash, _dataHashes.GetOrAdd(key, ""));
        _lastValidationTimes.TryAdd(key, DateTime.Now);
        _lastValidationTimes.TryUpdate(key, DateTime.Now, _lastValidationTimes.GetOrAdd(key, DateTime.MinValue));
    }

    public static string GetHash(string key)
    {
        return _dataHashes.TryGetValue(key, out string hash) ? hash : string.Empty;
    }

    public static DateTime GetLastValidationTime(string key)
    {
        return _lastValidationTimes.TryGetValue(key, out DateTime time) ? time : DateTime.MinValue;
    }

    public static void Clear()
    {
        _dataHashes.Clear();
        _lastValidationTimes.Clear();
    }
}