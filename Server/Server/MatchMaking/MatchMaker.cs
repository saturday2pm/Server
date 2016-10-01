using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    class MatchMaker
    {
        public static IMatchMaker Create<T>()
            where T : IMatchMaker, new()
        {
            var matchMaker = new T();

            var thread = new Thread(() => {
                MatchMakerThread(matchMaker);
            });
            thread.Start();

            return matchMaker;
        }

        private static void MatchMakerThread(IMatchMaker matchMaker)
        {
            while (true)
            {
                Match match = null;
                while (match != null)
                {
                    match = matchMaker.Poll();

                    // TODO
                    Thread.Sleep(10);
                }

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    matchMaker.callback?.Invoke(match);
                });
            }
        }
    }
}
