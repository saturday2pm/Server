using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ingame
{
    interface IMatchResolver
    {
        [ThreadSafe]
        Task RegisterMatch(string matchToken, MatchMaking.MatchData match);

        [ThreadSafe]
        Task<MatchMaking.MatchData> GetMatchInfo(string matchToken);
    }
}
