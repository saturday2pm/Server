using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Concurrency
{
    class ConcurrentSet<T>
    {
        private ConcurrentDictionary<T, T> dic { get; set; }

        public int Count
        {
            get
            {
                return dic.Count;
            }
        }

        public ConcurrentSet()
        {
            dic = new ConcurrentDictionary<T, T>();
        }

        public bool TryAdd(T obj)
        {
            return dic.TryAdd(obj, obj);
        }
        public bool Contains(T obj)
        {
            return dic.ContainsKey(obj);
        }
        public bool TryRemove(T obj)
        {
            T trash;
            return dic.TryRemove(obj, out trash);
        }

        public void Clear()
        {
            dic.Clear();
        }
    }
}
