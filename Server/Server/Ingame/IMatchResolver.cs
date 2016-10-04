using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ingame
{
    interface IMatchResolver
    {
        Task RegisterMatch(string matchToken, MatchMaking.MatchData match);

        Task<MatchMaking.MatchData> GetMatchInfo(string matchToken);
    }
}
