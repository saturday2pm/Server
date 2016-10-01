using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    class MatchMakerRedis : IMatchMaker
    {
        public Action<Match> callback
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Enqueue(int playerId)
        {
            throw new NotImplementedException();
        }

        public Match Poll()
        {
            throw new NotImplementedException();
        }
    }
}
