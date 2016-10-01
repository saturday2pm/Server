using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtocolCS;

namespace Server.AI
{
    /// <summary>
    /// 아무것도 안하는 봇
    /// </summary>
    class DummyAi : IAi
    {
        public void Initialize(Frame[] frames)
        {
            
        }

        public IngameEvent[] Process()
        {
            return new IngameEvent[] { };
        }
    }
}
