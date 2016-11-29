using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Server
{
    using Env;
    using MatchMaking;
    using Ingame;

    class Program
    {
        static void Main(string[] args)
        {
            ServerBuilder.Create()
                .WithService(ServiceNames.Ingame)
                .Start();
        }
    }
}
