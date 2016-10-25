using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ingame
{
    interface IMatchResolver
    {
        /// <summary>
        /// 이 메소드를 구현하여, 매치 정보를 어딘가에 기록한다.
        /// * 메모리 어딘가
        /// * DB 어딘가
        /// * 파일 어딘가
        /// </summary>
        /// <param name="matchToken">매치 토큰 (KEY)</param>
        /// <param name="match">매치 정보 (VALUE)</param>
        /// <returns></returns>
        [ThreadSafe]
        Task RegisterMatch(string matchToken, MatchMaking.MatchData match);

        /// <summary>
        /// 이 메소드를 구현하여, 어딘가에 저장한 매치 정보를 가져온다.
        /// </summary>
        /// <param name="matchToken">매치 토큰 (KEY)</param>
        /// <returns></returns>
        [ThreadSafe]
        Task<MatchMaking.MatchData> GetMatchInfo(string matchToken);
    }
}
