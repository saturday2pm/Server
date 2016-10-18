using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Env
{
    class ServerEnv
    {
        public static IEnv selectedEnv { get; private set; }

        static ServerEnv()
        {
#if BUILD_EC2
            selectedEnv = new EnvEC2();
#else
            selectedEnv = new EnvLocalWithoutDB();
#endif

            Console.WriteLine("SelectedEnv : " + selectedEnv);
        }
    }
}
