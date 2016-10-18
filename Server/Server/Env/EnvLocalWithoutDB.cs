using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Env
{
    class EnvLocalWithoutDB : IEnv
    {
        public string publicAddress => "localhost";
        public int port => 9916;
    }
}
