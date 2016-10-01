using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ingame
{
    public sealed partial class IngameService : Service<IngameService>
    {
        public static readonly string Path = "/game";
        
        static IngameService()
        {
            // IngameService.Match.cs
            InitMatch();
        }
    }
}
