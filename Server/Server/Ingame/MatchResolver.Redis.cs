using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.MatchMaking;

namespace Server.Ingame
{
    class MatchResolverRedis : IMatchResolver
    {
        public Task<MatchData> GetMatchInfo(string matchToken)
        {
            throw new NotImplementedException();
        }

        public Task RegisterMatch(string matchToken, MatchData match)
        {
            throw new NotImplementedException();
        }
    }
}
