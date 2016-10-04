using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ProtocolCS;

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
        /// 모든 플레이어로부터 Frame패킷이 도착했는지 조사한다.
        /// </summary>
        public bool canAggregate
        {
            get
            {
                return (players.Length - botPlayerCount) == eventArrived.Count;
            }
        }
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
        
        private ConcurrentBag<Frame> frameBuffer { get; set; }

        private int botPlayerCount;

        public GameProcessor(IngameService[] players)
        {
            this.players = players;
            
            this.eventBuffer = new List<IngameEvent>();
            this.boundTask = new BoundTask();
            this.frameBuffer = new ConcurrentBag<Frame>();

            this.currentFrameNo = 0;
            this.seed = 0;
        }

        public void AddEvent(int playerId, IngameEvent ev){
            if (eventArrived.TryAdd(playerId) == false)
                throw new InvalidOperationException("already has events");

            boundTask.Run(() => eventBuffer.Add(ev));
        }
        public void AddEvents(int playerId, IEnumerable<IngameEvent> ev)
        {
            if (eventArrived.TryAdd(playerId) == false)
                throw new InvalidOperationException("already has events");

            boundTask.Run(() => eventBuffer.AddRange(ev));
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
        public Task<IngameEvent[]> Step()
        {
            frameBuffer.Add(new Frame()
            {
                frameNo = currentFrameNo,
                events = eventBuffer.ToArray()
            });
            eventArrived.Clear();
            currentFrameNo++;

            return boundTask.Run(() => {
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
