using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;

namespace Server.MatchMaking
{
    using Ingame;

    public sealed class MatchMakingService : Service<MatchMakingService>
    {
        public static readonly string Path = "/mmaker";

        private static IMatchMaker matchMaker { get; set; }
        private static IMatchResolver matchResolver { get; set; }

        private ClientState clientState { get; set; }

        static MatchMakingService()
        {
            matchMaker = MatchMaker.Create<MatchMakerSimple>();
            matchResolver = MatchResolver.Create<MatchResolverSimple>();

            MatchMaker.onMatchCreated += OnMatchCreated;
        }

        public MatchMakingService()
        {
            clientState = ClientState.Ready;
        }

        /// <summary>
        /// 매치가 생성되면 자동으로 실행되는 콜백
        /// </summary>
        /// <param name="match">생성된 매치</param>
        private async static void OnMatchCreated(Match match)
        {
            Console.WriteLine("OnMatchCreated");

            var matchToken = Guid.NewGuid().ToString();
            var packet = new MatchSuccess()
            {
                gameServerAddress = "ws://localhost/game",
                matchToken = matchToken
            };

            Console.WriteLine($"MatchToken : {matchToken}");

            await matchResolver.RegisterMatch(matchToken, match);

            var players = match.playerIds.Select(x => GetSessionById(x));
            foreach (var player in players)
            {
                player.clientState = ClientState.MatchCreated;
                player.SendPacket(packet);
            }
        }

        public void OnJoinQueue(JoinQueue p)
        {
            Console.WriteLine("JoinQueue");

            if (clientState != ClientState.Ready)
                throw new InvalidOperationException("clientState != .Ready");

            currentPlayerId = p.senderId;
            matchMaker.Enqueue(p.senderId);

            clientState = ClientState.QueueJoined;
        }
        public void OnJoinBotQueue(JoinBotQueue p)
        {
            Console.WriteLine("JoinBotQueue");

            if (clientState != ClientState.Ready)
                throw new InvalidOperationException("clientState != .Ready");

            currentPlayerId = p.senderId;
            matchMaker.Enqueue(p.senderId);
            matchMaker.Enqueue(-1);

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
