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

        static MatchMakingService()
        {
            matchMaker = MatchMaker.Create<MatchMakerSimple>();
            matchResolver = MatchResolver.Create<MatchResolverSimple>();

            matchMaker.callback = OnMatchCreated;
        }

        public MatchMakingService()
        {

        }

        /// <summary>
        /// 매치가 생성되면 자동으로 실행되는 콜백
        /// </summary>
        /// <param name="match">생성된 매치</param>
        private async static void OnMatchCreated(Match match)
        {
            Console.WriteLine("OnMatchCreated");

            var matchId = new Guid().ToString();
            var packet = new MatchSuccess()
            {
                gameServerAddress = "ws://localhost/game",
                matchToken = matchId
            };

            Console.WriteLine($"MatchID : {matchId}");

            await matchResolver.RegisterMatch(matchId, match);

            var players = match.playerIds.Select(x => GetSessionById(x));
            foreach (var player in players)
            {
                player.SendPacket(packet);
            }
        }

        public void OnJoinQueue(JoinQueue p)
        {
            Console.WriteLine("JoinQueue");

            currentPlayerId = p.senderId;
            matchMaker.Enqueue(p.senderId);
        }
        public void OnLeaveQueue(LeaveQueue p)
        {
            Console.WriteLine("LeaveQueue");
        }
    }
}
