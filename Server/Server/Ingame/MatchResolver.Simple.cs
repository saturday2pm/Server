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
        private static ConcurrentDictionary<string, MatchMaking.Match> matches { get; set; }

        static MatchResolverSimple()
        {
            matches = new ConcurrentDictionary<string, Match>();
        }
        
        public Task<Match> GetMatchInfo(string matchToken)
        {
            Console.WriteLine("MatchResolverSimple::GetMatchInfo : " + matchToken);

            return Task.FromResult<Match>(matches[matchToken]);
        }

        public Task RegisterMatch(string matchToken, Match match)
        {
            Console.WriteLine("MatchResolverSimple::RegisterMatch : " + matchToken);

            matches[matchToken] = match;

            return Task.FromResult(0);
        }
    }
}
