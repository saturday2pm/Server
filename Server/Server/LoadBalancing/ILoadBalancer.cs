using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.LoadBalancing
{
    interface ILoadBalancer
    {
        void Initialize(string[] hosts);

        string GetNext();
    }
}
