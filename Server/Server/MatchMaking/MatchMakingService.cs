using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;
using ProtocolCS.Constants;

namespace Server.MatchMaking
{
    using Ingame;

    sealed class MatchMakingService : Service<MatchMakingService>
    {
        public static readonly string Path = "/mmaker";

        private static IMatchMaker matchMaker { get; set; }

        private ClientState clientState { get; set; }

        static MatchMakingService()
        {
            matchMaker = MatchMaker.Create<MatchMakerSimple>();
        }

        public MatchMakingService()
        {
            clientState = ClientState.Ready;
        }

        /// <summary>
        /// 매치가 생성되면 자동으로 실행되는 콜백
        /// </summary>
        /// <param name="match">생성된 매치</param>
        public void OnMatchCreated(string matchToken, MatchData match)
        {
            var packet = new MatchSuccess()
            {
                gameServerAddress = "ws://localhost/game",
                matchToken = matchToken
            };
            
            SendPacket(packet);

            clientState = ClientState.MatchCreated;
        }

        public void OnJoinQueue(JoinQueue p)
        {
            Console.WriteLine("JoinQueue");

            if (clientState != ClientState.Ready)
                throw new InvalidOperationException("clientState != .Ready");

            currentPlayerId = p.senderId;
            matchMaker.Enqueue(this);

            clientState = ClientState.QueueJoined;
        }
        public void OnJoinBotQueue(JoinBotQueue p)
        {
            Console.WriteLine("JoinBotQueue");

            if (clientState != ClientState.Ready)
                throw new InvalidOperationException("clientState != .Ready");

            currentPlayerId = p.senderId;
            matchMaker.Enqueue(this);

            clientState = ClientState.QueueJoined;
        }

        public void OnLeaveQueue(LeaveQueue p)
        {
            Console.WriteLine("LeaveQueue");

            if (clientState == ClientState.Ready ||
                clientState == ClientState.Closing)
                throw new InvalidOperationException("clientState != .QueueJoinned, .MatchCreated");

            clientState = ClientState.Ready;
        }
    }
}
