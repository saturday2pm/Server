using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ingame
{
    interface IMatchResolver
    {
        Task RegisterMatch(string matchToken, MatchMaking.Match match);

        Task<MatchMaking.Match> GetMatchInfo(string matchToken);
    }
}
