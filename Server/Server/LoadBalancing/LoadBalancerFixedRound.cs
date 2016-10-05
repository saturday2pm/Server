using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.LoadBalancing
{
    class LoadBalancerFixedRound : ILoadBalancer
    {
        private string[] hosts { get; set; }

        private uint cursor;

        public void Initialize(string[] hosts)
        {
            if (hosts == null || hosts.Length == 0)
                throw new ArgumentNullException(nameof(hosts));

            this.hosts = hosts;
        }

        public string GetNext()
        {
            unchecked
            {
                cursor++;

                return hosts[(cursor % hosts.Length)];
            }
        }
    }
}
