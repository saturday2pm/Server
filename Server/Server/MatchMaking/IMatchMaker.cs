using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    interface IMatchMaker
    {
        Action<Match> callback { get; set; }

        void Enqueue(int playerId);

        Match Poll();
    }

    interface IMatchQueue
    {
        void Enqueue(int playerId);

    }
}
