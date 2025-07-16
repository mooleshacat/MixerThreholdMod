

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MixerThreholdMod_1_0_0.Utils
{
    public class ThreadSafeList<T> : IEnumerable<T>
    {
        private readonly List<T> _list = new List<T>();
        private readonly object _lock = new object();

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _list.Count;
                }
            }
        }

        public void Add(T item)
        {
            lock (_lock)
            {
                _list.Add(item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
            }
        }

        public bool Any(Func<T, bool> predicate)
        {
            lock (_lock)
            {
                return _list.Any(predicate);
            }
        }

        public void RemoveAll(Func<T, bool> predicate)
        {
            lock (_lock)
            {
                _list.RemoveAll(item => predicate(item));
            }
        }

        public List<T> ToList()
        {
            lock (_lock)
            {
                return new List<T>(_list);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_lock)
            {
                return new List<T>(_list).GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}