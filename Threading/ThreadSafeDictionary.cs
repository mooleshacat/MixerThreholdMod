using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Provides thread-safe dictionary operations for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types and proper error handling.
/// </summary>
public class ThreadSafeDictionary<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public void Add(TKey key, TValue value)
    {
        _lock.EnterWriteLock();
        try
        {
            _dict[key] = value;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool Remove(TKey key)
    {
        _lock.EnterWriteLock();
        try
        {
            return _dict.Remove(key);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        _lock.EnterReadLock();
        try
        {
            return _dict.TryGetValue(key, out value);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public int Count
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _dict.Count;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _dict.Clear();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        _lock.EnterReadLock();
        try
        {
            return new Dictionary<TKey, TValue>(_dict);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
}