using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;

namespace Server.Ingame
{
    using Concurrency;

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
                return players.Length == eventArrived.Count;
            }
        }

        private BoundTask boundTask { get; set; }

        private HashSet<int> eventArrived { get; set; }
        

        public GameProcessor(IngameService[] players)
        {
            this.players = players;
            
            this.eventBuffer = new List<IngameEvent>();
            this.boundTask = new BoundTask();

            this.currentFrameNo = 0;
            this.seed = 0;
        }

        public void AddEvent(int playerId, IngameEvent ev){
            boundTask.Run(() => eventBuffer.Add(ev));
            eventArrived.Add(playerId);
        }
        public void AddEvents(int playerId, IEnumerable<IngameEvent> ev)
        {
            boundTask.Run(() => eventBuffer.AddRange(ev));
            eventArrived.Add(playerId);
        }

        public Task<IngameEvent[]> Aggregate()
        {
            eventArrived.Clear();
            return boundTask.Run(() => eventBuffer.ToArray());
        }
    }
}
