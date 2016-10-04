using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    class MatchData
    {
        public int[] playerIds { get; protected set; }

        public MatchData(int[] playerIds)
        {
            this.playerIds = playerIds;
        }
        public MatchData()
        {
        }
    }
    class MatchDataInternal : MatchData
    {
        public MatchMakingService[] players { get; private set; }

        public MatchDataInternal(MatchMakingService[] players)
        {
            this.players = players;
            this.playerIds = players
                .Select(x => x.currentPlayerId).ToArray();
        }
    }
}
