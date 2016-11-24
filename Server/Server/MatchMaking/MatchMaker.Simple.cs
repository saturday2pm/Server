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
            normalQueue = new MatchQueueSimple(2);
            botQueue = new MatchQueueSimple(2);
        }

        public void Enqueue(MatchPlayer player, QueueType queueType)
        {
            switch (queueType)
            {
                case QueueType.Normal:
                    normalQueue.Enqueue(player);
                    break;

                case QueueType.BotGame:
                    // TODO : 동시입장 문제 
                    botQueue.Enqueue(player);
                    botQueue.Enqueue(MatchPlayer.CreateBot());
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
