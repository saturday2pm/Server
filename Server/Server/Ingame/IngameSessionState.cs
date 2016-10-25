using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ingame
{
    enum IngameSessionState
    {
        /// <summary>
        /// 연결된 직후의 상태
        /// </summary>
        Connected,

        /// <summary>
        /// 자기는 매치에 등록 완료하고,
        /// 다른 플레이어가 들어오길 기다리는 상태
        /// </summary>
        WaitingForStart,

        /// <summary>
        /// 자기가 등록 완료된 상태에서,
        /// 매치가 취소됨
        /// </summary>
        MatchCanceled,

        /// <summary>
        /// 게임 플레이 중
        /// </summary>
        Playing
    }
}
