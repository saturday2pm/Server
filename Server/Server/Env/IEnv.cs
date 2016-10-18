using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Env
{
    interface IEnv
    {
        string publicAddress { get; }
        int port { get; }

        //bool useSecureSocket { get; }
    }
}
