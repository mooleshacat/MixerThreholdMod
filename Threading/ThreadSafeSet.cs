

using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Provides thread-safe set operations for MixerThreholdMod.
///  THREAD SAFETY: All operations are thread-safe.
///  .NET 4.8.1 COMPATIBLE: Uses explicit types and proper error handling.
/// </summary>
public class ThreadSafeSet<T>
{
    private readonly HashSet<T> _set = new HashSet<T>();
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public bool Add(T item)
    {
        _lock.EnterWriteLock();
        try
        {
            return _set.Add(item);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool Remove(T item)
    {
        _lock.EnterWriteLock();
        try
        {
            return _set.Remove(item);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool Contains(T item)
    {
        _lock.EnterReadLock();
        try
        {
            return _set.Contains(item);
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
                return _set.Count;
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
            _set.Clear();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public T[] ToArray()
    {
        _lock.EnterReadLock();
        try
        {
            return new List<T>(_set).ToArray();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
}