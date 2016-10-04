using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;

namespace Server.Ingame
{
    public partial class IngameService : Service<IngameService>
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

            // 첫번째 입장자가 아니면, 이미 만들어진값 가져옴
            if (activeMatches.TryAdd(p.matchToken, matchProcessor) == false)
                matchProcessor = activeMatches[p.matchToken];

            // 풀방됨
            if (matchProcessor.Join(this))
            {
                if (matchProcessor.CanStartGame())
                {
                    var packet = new StartGame()
                    {
                        players = matchProcessor.players
                            .Select(x => x.AsPlayer()).ToArray(),
                        seed = 0
                    };

                    matchProcessor.players.Broadcast(packet);
                    
                    matchProcessor.matchState = MatchState.Started;
                }
                else
                {
                    var packet = new CancelGame()
                    {
                    };
                    matchProcessor.players.Broadcast(packet);

                    matchProcessor.matchState = MatchState.Canceled;
                }
            }
        }
    }
}
