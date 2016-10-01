using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    using Constants;

    class MatchMakerSimple : IMatchMaker
    {
        public Action<Match> callback { get; set; }

        private ConcurrentQueue<int> queue { get; set; }
        private int targetPlayersForNextMatch { get; set; }

        public MatchMakerSimple()
        {
            queue = new ConcurrentQueue<int>();

            ResetTargetPlayerCount();
        }

        public void Enqueue(int playerId)
        {
            queue.Enqueue(playerId);
        }

        public Match Poll()
        {
            if (queue.Count <= targetPlayersForNextMatch)
                return null;

            int[] players = new int[targetPlayersForNextMatch];
            for(int i = 0; i < players.Length; i++)
            {
                if (queue.TryDequeue(out players[i]) == false)
                    throw new InvalidOperationException("something went wrong");
            }

            return new Match()
            {
                playerIds = players
            };
        }

        private void ResetTargetPlayerCount()
        {
            if (Constants.ImmediateMatching)
            {
                targetPlayersForNextMatch = Constants.MinPlayerForGame;
                return;
            }

            targetPlayersForNextMatch = new Random()
                .Next(Constants.MinPlayerForGame, Constants.MaxPlayerForGame);
        }
    }
}
