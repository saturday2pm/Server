using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    enum QueueType
    {
        Elo_0_500,
        Elo_501_1000,
        Elo_1001_1500,
        Elo_1501_2000,
        Elo_2001_9999,

        BotGame,
    }
}
