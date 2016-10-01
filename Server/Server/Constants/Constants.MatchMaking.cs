using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Constants
{
    partial class Constants
    {
        /// <summary>
        /// 게임 시작까지 최소 몇명 기다려야하는지
        /// </summary>
        public static readonly int MinPlayerForGame = 2;
        
        /// <summary>
        /// 최대 몇명까지 참여할 수 있는지
        /// </summary>
        public static readonly int MaxPlayerForGame = 3;

        /// <summary>
        /// true일 경우 기다리지 않고 즉시 게임을 만든다.
        ///  -> 항상 Min 으로 작동한다는 뜻
        /// </summary>
        public static readonly bool ImmediateMatching = true;
    }
}
