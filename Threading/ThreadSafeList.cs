

using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Provides thread-safe list operations for MixerThreholdMod.
/// âš ï¸ THREAD SAFETY: All operations are thread-safe.
/// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types and proper error handling.
/// </summary>
public class ThreadSafeList<T>
{
    private readonly List<T> _list = new List<T>();
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public void Add(T item)
    {
        _lock.EnterWriteLock();
        try
        {
            _list.Add(item);
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
            return _list.Remove(item);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public T Get(int index)
    {
        _lock.EnterReadLock();
        try
        {
            return _list[index];
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
                return _list.Count;
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
            _list.Clear();
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
            return _list.ToArray();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
}