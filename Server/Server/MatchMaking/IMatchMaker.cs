using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    interface IMatchMaker
    {
        void Enqueue(MatchMakingService player);

        MatchDataInternal Poll();
    }

    interface IMatchQueue
    {
        void Enqueue(int playerId);

    }
}
