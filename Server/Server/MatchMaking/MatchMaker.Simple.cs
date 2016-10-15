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
        IMatchQueue normalQueue { get; set; }
        IMatchQueue botQueue { get; set; }

        public MatchMakerSimple()
        {
            normalQueue = new MatchQueueSimple();
            botQueue = new MatchQueueSimple();
        }

        public void Enqueue(MatchMakingService player, QueueType queueType)
        {
            switch (queueType)
            {
                case QueueType.Normal:
                    normalQueue.Enqueue(player);
                    break;

                case QueueType.BotGame:
                    botQueue.Enqueue(player);
                    break;

                case QueueType.Nan2:
                    break;
            }
        }

        public IEnumerable<MatchDataInternal> Poll()
        {
            var matchData = normalQueue.Poll();
            if (matchData != null)
                yield return matchData;

            matchData = botQueue.Poll();
            if (matchData != null)
                yield return matchData;
        }
    }
}
