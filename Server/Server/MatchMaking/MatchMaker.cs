using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.MatchMaking
{
    using Ingame;

    class MatchMaker
    {
        public static IMatchMaker Create<T>()
            where T : IMatchMaker, new()
        {
            var matchMaker = new T();
            var matchResolver = new MatchResolverSimple();

            var thread = new Thread(() => {
                MatchMakerThread(matchMaker, matchResolver);
            });
            thread.Start();

            return matchMaker;
        }

        private static void MatchMakerThread(IMatchMaker matchMaker, IMatchResolver matchResolver)
        {
            while (true)
            {
                MatchDataInternal match = null;
                while (match == null)
                {
                    match = matchMaker.Poll();

                    // TODO
                    Thread.Sleep(10);
                }

                ThreadPool.QueueUserWorkItem(async _ =>
                {
                    var matchToken = Guid.NewGuid().ToString();

                    await matchResolver.RegisterMatch(matchToken, match);
                    
                    foreach (var player in match.players)
                        player.OnMatchCreated(matchToken, match);
                });
            }
        }
    }
}
