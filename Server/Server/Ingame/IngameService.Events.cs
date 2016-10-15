using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;

namespace Server.Ingame
{
    partial class IngameService : Service<IngameService>
    {
        internal GameProcessor gameProcessor { get; set; }

        /// <summary>
        /// StartGame이 완료되고 게임이 시작되기 전에 호출되는 메소드
        /// </summary>
        /// <param name="gameProcessor">게임 프로세서</param>
        private void InitializeGame(GameProcessor gameProcessor)
        {
            if (this.gameProcessor != null)
                throw new InvalidOperationException($"already has {gameProcessor}");

            this.gameProcessor = gameProcessor;
        }

        public async void OnFrame(Frame p)
        {
            Console.WriteLine("OnFrame");

            EnsureGameStarted();

            if (p.frameNo > gameProcessor.currentFrameNo)
                throw new ArgumentOutOfRangeException("p.frameNo > currentFrameNo");

            if (gameProcessor.AddEvents(currentPlayerId, p.events))
            {
                // 이번 프레임 브로드캐스팅
                var frameNo = gameProcessor.currentFrameNo;
                var aggregatedEvents = await gameProcessor.Step();
                var packet = new Frame()
                {
                    frameNo = frameNo,
                    events = aggregatedEvents
                };

                gameProcessor.players.Broadcast(packet);
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
