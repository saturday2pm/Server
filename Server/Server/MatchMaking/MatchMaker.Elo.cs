using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    class MatchMakerElo : IMatchMaker
    {
        IMatchQueue elo_0_500 { get; set; }
        IMatchQueue elo_501_1000 { get; set; }
        IMatchQueue elo_1001_1500 { get; set; }
        IMatchQueue elo_1501_2000 { get; set; }
        IMatchQueue elo_2001_9999 { get; set; }
        IMatchQueue botQueue { get; set; }

        public void Enqueue(MatchMakingService player, QueueType queueType)
        {
            throw new NotImplementedException();
        }

        IEnumerable<MatchDataInternal> IMatchMaker.Poll()
        {
            throw new NotImplementedException();
        }
    }
}
