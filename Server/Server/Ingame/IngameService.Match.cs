using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;

namespace Server.Ingame
{
    partial class IngameService : Service<IngameService>
    {
        private static IMatchResolver matchResolver { get; set; }
        private static ConcurrentDictionary<string, MatchProcessor> activeMatches { get; set; }
        
        private static void InitMatch()
        {
            matchResolver = MatchResolver.Create<MatchResolverSimple>();
            activeMatches = new ConcurrentDictionary<string, MatchProcessor>();
        }

        public async void OnJoinGame(JoinGame p)
        {
            currentPlayerId = p.senderId;

            var match = await matchResolver.GetMatchInfo(p.matchToken);
            var matchProcessor = new MatchProcessor(match);
            var gameProcessor = new GameProcessor(matchProcessor.players);

            // 첫번째 입장자가 아니면, 이미 만들어진값 가져옴
            if (activeMatches.TryAdd(p.matchToken, matchProcessor) == false)
                matchProcessor = activeMatches[p.matchToken];

            // 풀방됨
            if (matchProcessor.Join(this))
            {
                if (matchProcessor.CanStartGame())
                    PrepareGame(matchProcessor);
                else
                    CancelGame(matchProcessor);
            }
        }

        /// <summary>
        /// 매치 프로세서가 할 일을 모두 마치고, 실제 게임 가능한 상태를 준비한다.
        /// 매치에 속한 플레어에게 시작 알림을 보낸다.
        /// </summary>
        /// <param name="matchProcessor"></param>
        private static void PrepareGame(MatchProcessor matchProcessor)
        {
            var packet = new StartGame()
            {
                players = matchProcessor.players
                            .Select(x => x.AsPlayer()).ToArray(),
                seed = 0
            };

            matchProcessor.players.Broadcast(packet);

            var gameProcesssor = matchProcessor.Start();
            foreach (var player in matchProcessor.players)
            {
                player.InitializeGame(gameProcesssor);
            }
        }

        /// <summary>
        /// 매치에 속한 플레이어에게 취소 알림을 보낸다.
        /// </summary>
        /// <param name="matchProcessor"></param>
        private static void CancelGame(MatchProcessor matchProcessor)
        {
            var packet = new CancelGame()
            {
            };
            matchProcessor.players.Broadcast(packet);

            matchProcessor.Cancel();
        }
    }
}
