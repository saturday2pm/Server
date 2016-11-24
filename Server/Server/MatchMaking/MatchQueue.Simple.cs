using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    using Constants;

    class MatchQueueSimple : IMatchQueue
    {
        private ConcurrentQueue<MatchPlayer> queue { get; set; }
        private object queueEnqueueLock = new object();

        private int targetPlayersForNextMatch { get; set; }

        private int playersPerMatch;

        public MatchQueueSimple(int playersPerMatch)
        {
            this.playersPerMatch = playersPerMatch;
            this.queue = new ConcurrentQueue<MatchMakingService>();

            ResetTargetPlayerCount();
        }

        public void Enqueue(MatchPlayer player)
        {
            lock (queueEnqueueLock)
                queue.Enqueue(player);
        }

        public MatchDataInternal Poll()
        {
            if (queue.Count < targetPlayersForNextMatch)
                return null;

            var players = new MatchPlayer[targetPlayersForNextMatch];
            for (int i = 0; i < players.Length; i++)
            {
                if (queue.TryDequeue(out players[i]) == false)
                    throw new InvalidOperationException("something went wrong");

                // leave players
            }

            return new MatchDataInternal(players);
        }

        private void ResetTargetPlayerCount()
        {
            if (Constants.ImmediateMatching)
            {
                targetPlayersForNextMatch = Constants.MinPlayerForGame;
                return;
            }

            targetPlayersForNextMatch = playersPerMatch;
            //targetPlayersForNextMatch = new Random()
            //    .Next(Constants.MinPlayerForGame, Constants.MaxPlayerForGame);
        }
    }
}
