using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.LoadBalancing
{
    class LoadBalancerFlexibleRound : ILoadBalancer
    {
        private ConcurrentQueue<string> hosts;

        public void Initialize(string[] hosts)
        {
            if (hosts == null || hosts.Length == 0)
                throw new ArgumentNullException(nameof(hosts));

            foreach(var host in hosts)
            {
                this.hosts.Enqueue(host);
            }
        }

        public string GetNext()
        {
            string host = null;

            Retry:

            if (hosts.TryDequeue(out host))
            {
                hosts.Enqueue(host);
                return host;
            }
            else
            {
                Thread.SpinWait(10);
                goto Retry;
            }
        }
    }
}
