using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;

namespace Server.AI
{
    interface IAi
    {
        /// <summary>
        /// AI 를 초기화한다.
        /// </summary>
        /// <param name="frames">이전 동작들</param>
        void Initialize(Frame[] frames);

        /// <summary>
        /// 실행하고, 이번턴의 동작들을 가져온다.
        /// </summary>
        /// <returns>이번 턴 동작들</returns>
        IngameEvent[] Process();
    }
}
