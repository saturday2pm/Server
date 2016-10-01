using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;

namespace Server.Ingame
{
    public sealed partial class IngameService : Service<IngameService>
    {
        private GameProcessor gameProcessor { get; set; }

        public async void OnFrame(Frame p)
        {
            Console.WriteLine("OnFrame");

            EnsureGameStarted();

            gameProcessor.AddEvents(currentPlayerId, p.events);
            if (gameProcessor.canAggregate)
            {
                // 이번 프레임 브로드캐스팅
                var aggregatedEvents = await gameProcessor.Aggregate();
                var packet = new Frame()
                {
                    frameNo = gameProcessor.currentFrameNo,
                    events = aggregatedEvents
                };

                foreach(var player in gameProcessor.players)
                    player.SendPacket(packet);
            }
        }

        private void EnsureGameStarted()
        {
            // 게임 시작 안된 상태에서 이벤트 보냄
            if (gameProcessor == null)
                throw new InvalidOperationException("gameProcessor -> null");
        }
    }
}
