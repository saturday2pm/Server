using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.MatchMaking;

namespace Server.Ingame
{
    class MatchResolverSimple : IMatchResolver
    {
        private Dictionary<string, MatchMaking.Match> matches { get; set; }

        public MatchResolverSimple()
        {
            matches = new Dictionary<string, Match>();
        }

        public Task<Match> GetMatchInfo(string matchToken)
        {
            return Task.FromResult<Match>(matches[matchToken]);
        }

        public Task RegisterMatch(string matchToken, Match match)
        {
            matches[matchToken] = match;

            return Task.FromResult(0);
        }
    }
}
