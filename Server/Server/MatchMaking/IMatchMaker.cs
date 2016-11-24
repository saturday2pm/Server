using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GSF;
using GSF.Concurrency;

using ProtocolCS;
using ProtocolCS.Constants;

namespace Server.MatchMaking
{
    class MatchPlayer
    {
        public int userId { get; private set; }
        public MatchMakingService session { get; private set; }

        public static MatchPlayer CreateBot()
        {
            return new MatchPlayer(ReservedPlayerId.Bot);
        }
        public static MatchPlayer FromSession(MatchMakingService session)
        {
            return new MatchPlayer(session);
        }

        private MatchPlayer(int botUserId)
        {
            this.userId = botUserId;
        }
        private MatchPlayer(MatchMakingService session)
        {
            this.userId = session.UserId;
            this.session = session;
        }
    }

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
        void Enqueue(MatchPlayer player, QueueType queueType);

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
        void Enqueue(MatchPlayer player);

        [NotThreadSafe(As.SingleConsumer)]
        MatchDataInternal Poll();
    }
}
