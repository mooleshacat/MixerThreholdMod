using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Reads mixer data from disk with crash prevention and error handling.
/// ⚠️ THREAD SAFETY: All operations are thread-safe and async.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses Task.Run, string.Format, and proper cancellation tokens.
/// </summary>
public static class MixerDataReader
{
    public static async Task<Dictionary<int, float>> ReadMixerDataAsync(string filePath, CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Main.logger?.Warn(1, string.Format("{0} MixerDataReader: File not found: {1}", PERSISTENCE_PREFIX, filePath));
                return new Dictionary<int, float>();
            }

            string json = await Task.Run(() => File.ReadAllText(filePath, Encoding.UTF8), cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrEmpty(json))
            {
                Main.logger?.Warn(1, string.Format("{0} MixerDataReader: File empty: {1}", PERSISTENCE_PREFIX, filePath));
                return new Dictionary<int, float>();
            }

            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (data == null || !data.ContainsKey(MIXER_VALUES_KEY))
            {
                Main.logger?.Warn(1, string.Format("{0} MixerDataReader: Invalid data format: {1}", PERSISTENCE_PREFIX, filePath));
                return new Dictionary<int, float>();
            }

            var mixerValues = JsonConvert.DeserializeObject<Dictionary<int, float>>(data[MIXER_VALUES_KEY].ToString());
            return mixerValues ?? new Dictionary<int, float>();
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} MixerDataReader: Read failed: {1}", PERSISTENCE_PREFIX, ex.Message));
            return new Dictionary<int, float>();
        }
    }
}