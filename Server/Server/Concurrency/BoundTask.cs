using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Concurrency
{
    class BoundTask
    {
        public int id { get; set; }

        public BoundTask()
        {
            id = BoundTaskPool.GenerateNextId();
        }

        [ThreadSafe]
        public Task<T> Run<T>(Func<T> task)
        {
            return BoundTaskPool.Enqueue<T>(id, task);
        }

        [ThreadSafe]
        public Task Run(Action task)
        {
            return BoundTaskPool.Enqueue(id, task);
        }
    }
}
