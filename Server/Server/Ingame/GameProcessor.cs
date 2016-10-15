using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ProtocolCS;
using ProtocolCS.Constants;

namespace Server.Ingame
{
    using Concurrency;
    using AI;

    class GameProcessor
    {
        public List<IngameEvent> eventBuffer { get; set; }
        public int currentFrameNo { get; set; }

        public int seed { get; private set; }

        public IngameService[] players { get; private set; }

        /// <summary>
        /// 플레이어 다나가고, 봇만 남아있는 좀비방인지
        /// </summary>
        public bool isZombieGame
        {
            get
            {
                return players.Length == botPlayerCount;
            }
        }

        private BoundTask boundTask { get; set; }
        private ConcurrentSet<int> eventArrived { get; set; }
        private int eventArrivedCount;
        private ConcurrentBag<Frame> frameBuffer { get; set; }

        private int lastFrameTick;

        private int botPlayerCount;

        public GameProcessor(IngameService[] players)
        {
            this.players = players;

            this.eventArrived = new ConcurrentSet<int>();
            this.eventBuffer = new List<IngameEvent>();
            this.boundTask = new BoundTask();
            this.frameBuffer = new ConcurrentBag<Frame>();

            this.currentFrameNo = 0;
            this.seed = 0;

            this.lastFrameTick = Environment.TickCount;
        }

        public bool AddEvent(int playerId, IngameEvent ev){
            bool lastPlayer = Interlocked.Increment(ref eventArrivedCount) == players.Length - botPlayerCount;
            if (eventArrived.TryAdd(playerId) == false)
                throw new InvalidOperationException("already has events");

            boundTask.Run(() => eventBuffer.Add(ev));

            if (lastPlayer)
                return true;
            return false;
        }
        public bool AddEvents(int playerId, IEnumerable<IngameEvent> ev)
        {
            bool lastPlayer = Interlocked.Increment(ref eventArrivedCount) == players.Length - botPlayerCount;
            if (eventArrived.TryAdd(playerId) == false)
                throw new InvalidOperationException("already has events");
            
            boundTask.Run(() => eventBuffer.AddRange(ev));

            if (lastPlayer)
                return true;
            return false;
        }
        public void ToAutoPlayer(int playerId)
        {
            Interlocked.Increment(ref botPlayerCount);
            boundTask.Run(() => {
                for (int i=0;i<players.Length;i++)
                {
                    if (players[i].currentPlayerId == playerId)
                        players[i] = AutoPlayer.MigrateFrom(players[i]);
                }
            });
        }

        /// <summary>
        /// 현재 프레임을 끝내고, 다음 프레임을 준비한다.
        /// </summary>
        /// <returns>이번 프레임에서 발행한 이벤트들</returns>
        public async Task<IngameEvent[]> Step()
        {
            frameBuffer.Add(new Frame()
            {
                frameNo = currentFrameNo,
                events = eventBuffer.ToArray()
            });
            eventArrived.Clear();
            Interlocked.Exchange(ref eventArrivedCount, 0);
            currentFrameNo++;

            var delay = FrameRate.Interval - (Environment.TickCount - lastFrameTick);
            if (delay > 0)
                await Task.Delay(delay);
            lastFrameTick = Environment.TickCount;

            return await boundTask.Run(() => {
                foreach(var player in players)
                {
                    if (player.isBotPlayer)
                        eventBuffer.AddRange(((AutoPlayer)player).ProcessTurn());
                }
                var events = eventBuffer.ToArray();
                eventBuffer.Clear();
                return events;
            });
        }

        public Frame[] GetPreviousFrames()
        {
            // ****TODO**** : LOCK
            Frame[] ary = new Frame[currentFrameNo];
            frameBuffer.CopyTo(ary, 0);
            return ary;
        }
    }
}
