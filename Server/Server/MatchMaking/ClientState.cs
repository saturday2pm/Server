using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    /// <summary>
    /// 클라이언트의 매칭 상태를 나타내는 집합
    /// </summary>
    enum ClientState
    {
        /// <summary>
        /// 큐에 조인 요청을 할 수 있는 상태
        /// </summary>
        Ready,

        /// <summary>
        /// 큐에 조인됨
        /// </summary>
        QueueJoined,

        /// <summary>
        /// 매칭 성공 상태,
        /// 큐에서 나간 상태임
        /// </summary>
        MatchCreated,

        /// <summary>
        /// 서버에서 닫는 상태
        /// </summary>
        Closing
    }
}
