using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ProtocolCS;
using ProtocolCS.Constants;

using GSF;

namespace Server.MatchMaking
{
    using Env;
    using Ingame;
    using LoadBalancing;

    sealed class MatchMakingService : Service<MatchMakingService>
    {
        public static readonly string Path = "/mmaker";

        private static ILoadBalancer loadBalancer { get; set; }
        private static IMatchMaker matchMaker { get; set; }

        private ClientState clientState { get; set; }

        static MatchMakingService()
        {
            loadBalancer = new LoadBalancerFixedRound();
            matchMaker = MatchMaker.Create<MatchMakerSimple>();

            // TODO : 로드밸런싱도 env가 가져감
            var ev = ServerEnv.selectedEnv;
            loadBalancer.Initialize(new string[] {
                $"ws://{ev.publicAddress}:{ev.port}/game"
            });
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
                gameServerAddress = loadBalancer.GetNext(),
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

            UserId = p.senderId;
            matchMaker.Enqueue(MatchPlayer.FromSession(this), QueueType.Normal);

            clientState = ClientState.QueueJoined;
        }
        public void OnJoinBotQueue(JoinBotQueue p)
        {
            Console.WriteLine("JoinBotQueue");

            if (clientState != ClientState.Ready)
                throw new InvalidOperationException("clientState != .Ready");

            UserId = p.senderId;
            matchMaker.Enqueue(MatchPlayer.FromSession(this), QueueType.BotGame);

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
