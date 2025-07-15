using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Manages mixer instances, values, and configuration for MixerThreholdMod.
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and use async patterns.
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
    /// ⚠️ MAIN THREAD WARNING: Never blocks Unity main thread; all I/O and heavy operations are async.
    /// </summary>
    internal class MixerManager
    {
        private readonly object _lock = new object();
        private readonly ConcurrentDictionary<int, float> _mixerValues = new ConcurrentDictionary<int, float>();
        private readonly HashSet<int> _activeMixerIds = new HashSet<int>();

        /// <summary>
        /// Adds or updates a mixer value.
        /// </summary>
        public void SetMixerValue(int mixerId, float value)
        {
            lock (_lock)
            {
                _mixerValues[mixerId] = value;
                _activeMixerIds.Add(mixerId);
                Main.logger?.Msg(2, string.Format("{0} SetMixerValue: Mixer {1} set to {2:F3}", MIXER_MANAGER_PREFIX, mixerId, value));
            }
        }

        /// <summary>
        /// Gets the value for a mixer, or returns 0 if not found.
        /// </summary>
        public float GetMixerValue(int mixerId)
        {
            lock (_lock)
            {
                return _mixerValues.TryGetValue(mixerId, out float value) ? value : 0f;
            }
        }

        /// <summary>
        /// Removes a mixer from management.
        /// </summary>
        public void RemoveMixer(int mixerId)
        {
            lock (_lock)
            {
                _mixerValues.TryRemove(mixerId, out _);
                _activeMixerIds.Remove(mixerId);
                Main.logger?.Msg(2, string.Format("{0} RemoveMixer: Mixer {1} removed", MIXER_MANAGER_PREFIX, mixerId));
            }
        }

        /// <summary>
        /// Gets all active mixer IDs.
        /// </summary>
        public IReadOnlyCollection<int> GetActiveMixerIds()
        {
            lock (_lock)
            {
                return new List<int>(_activeMixerIds).AsReadOnly();
            }
        }

        /// <summary>
        /// Loads mixer values asynchronously from persistence.
        /// </summary>
        public async Task LoadMixerValuesAsync()
        {
            try
            {
                var loadedValues = await Helpers.MixerDataPersistenceManager.LoadMixerDataAsync().ConfigureAwait(false);
                lock (_lock)
                {
                    _mixerValues.Clear();
                    foreach (var kvp in loadedValues)
                    {
                        _mixerValues[kvp.Key] = kvp.Value;
                        _activeMixerIds.Add(kvp.Key);
                    }
                }
                Main.logger?.Msg(1, string.Format("{0} Loaded mixer values from persistence.", MIXER_MANAGER_PREFIX));
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} Error loading mixer values: {1}\n{2}", MIXER_MANAGER_PREFIX, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Persists current mixer values asynchronously.
        /// </summary>
        public async Task PersistMixerValuesAsync()
        {
            try
            {
                Dictionary<int, float> valuesToSave;
                lock (_lock)
                {
                    valuesToSave = new Dictionary<int, float>(_mixerValues);
                }
                await Helpers.MixerDataPersistenceManager.PersistMixerDataAsync(valuesToSave).ConfigureAwait(false);
                Main.logger?.Msg(1, string.Format("{0} Persisted mixer values to disk.", MIXER_MANAGER_PREFIX));
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} Error persisting mixer values: {1}\n{2}", MIXER_MANAGER_PREFIX, ex.Message, ex.StackTrace));
            }
        }
    }
}