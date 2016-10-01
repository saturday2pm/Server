using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    class Elo
    {
        private static readonly int K = 15;

        public static int Update(int prev, int opponent, bool won)
        {
            var expected = 1 / (1 + Math.Pow(10, ((prev - opponent) / 400)));

            return (int)Math.Round(prev + K * (won ? 1 : 0 - expected));

        }
    }
}
