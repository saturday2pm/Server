using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.MatchMaking;

namespace Server.Ingame
{
    class MatchResolverRedis : MatchResolver
    {
        public Task<Match> GetMatchInfo(string matchToken)
        {
            throw new NotImplementedException();
        }

        public Task RegisterMatch(string matchToken, Match match)
        {
            throw new NotImplementedException();
        }
    }
}
