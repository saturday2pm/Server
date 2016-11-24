using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    /// <summary>
    /// 매치 서버 외부 /매치큐DB, 혹은 다른 서비스/ 로 전달될 수 있는 데이터
    /// </summary>
    class MatchData
    {
        public int[] playerIds { get; protected set; }

        public MatchData(int[] playerIds)
        {
            this.playerIds = playerIds;
        }
        public MatchData()
        {
        }
    }

    /// <summary>
    /// 매치 서버 내부적으로 가지고 있는 데이터
    /// </summary>
    class MatchDataInternal : MatchData
    {
        public MatchPlayer[] players { get; private set; }

        public MatchDataInternal(MatchPlayer[] players)
        {
            this.players = players;
            this.playerIds = players
                .Select(x => x.userId).ToArray();
        }
    }
}
