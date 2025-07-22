

using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Provides thread-safe queue operations for MixerThreholdMod.
///  THREAD SAFETY: All operations are thread-safe.
///  .NET 4.8.1 COMPATIBLE: Uses explicit types and proper error handling.
/// </summary>
public class ThreadSafeQueue<T>
{
    private readonly Queue<T> _queue = new Queue<T>();
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public void Enqueue(T item)
    {
        _lock.EnterWriteLock();
        try
        {
            _queue.Enqueue(item);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool TryDequeue(out T item)
    {
        _lock.EnterWriteLock();
        try
        {
            if (_queue.Count > 0)
            {
                item = _queue.Dequeue();
                return true;
            }
            item = default(T);
            return false;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public int Count
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _queue.Count;
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
            _queue.Clear();
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
            return _queue.ToArray();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
}