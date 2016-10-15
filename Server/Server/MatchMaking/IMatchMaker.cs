using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    interface IMatchMaker
    {
        /// <summary>
        /// 매치 메이커에 유저를 넣는다.
        /// 
        /// 매치 메이커는 유저의 데이터를 기반으로 적절한 MatchQueue에
        /// 다시 넣어야 한다.
        /// </summary>
        /// <param name="player">유저</param>
        /// <param name="queueType">큐 힌트</param>
        [ThreadSafe(As.MultiProducer)]
        void Enqueue(MatchMakingService player, QueueType queueType);

        [NotThreadSafe(As.SingleConsumer)]
        IEnumerable<MatchDataInternal> Poll();
    }

    interface IMatchQueue
    {
        /// <summary>
        /// 매치 큐에 유저를 넣는다.
        /// </summary>
        /// <param name="player">유저</param>
        [ThreadSafe(As.MultiProducer)]
        void Enqueue(MatchMakingService player);

        [NotThreadSafe(As.SingleConsumer)]
        MatchDataInternal Poll();
    }
}
