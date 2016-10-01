using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;

namespace Server.Ingame
{
    public partial class IngameService : Service<IngameService>
    {
        private static IMatchResolver matchResolver { get; set; }
        
        private static void InitMatch()
        {
            matchResolver = MatchResolver.Create<MatchResolverSimple>();
        }

        public async void OnJoinGame(JoinGame p)
        {
            currentPlayerId = p.senderId;

            var match = await matchResolver.GetMatchInfo(p.matchToken);

            MatchProcessor.Join(p.senderId);

            // TODO : 풀방되면 처리
        }
    }
}
