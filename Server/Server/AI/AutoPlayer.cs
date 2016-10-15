using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;
using ProtocolCS.Constants;

// jwvggwvvg
namespace Server.AI
{
    sealed class AutoPlayer : Ingame.IngameService
    {
        IAi ai { get; set; }

        /// <summary>
        /// 나간 플레이어 정보로부터, 오토 플레이어를 만든다.
        /// </summary>
        /// <param name="previousPlayer">나간 플레이어</param>
        /// <returns>새 오토 플레이어</returns>
        public static AutoPlayer MigrateFrom(Ingame.IngameService previousPlayer)
        {
            var player = new AutoPlayer();
            player.gameProcessor = previousPlayer.gameProcessor;
            player.currentPlayerId = previousPlayer.currentPlayerId;
            player.ai.Initialize(player.gameProcessor.GetPreviousFrames());

            return player;
        }


        public static AutoPlayer Create()
        {
            var player = new AutoPlayer();
            player.currentPlayerId = ReservedPlayerId.Bot;
            player.ai.Initialize(new Frame[] { });

            return player;
        }

        private AutoPlayer()
        {
            isBotPlayer = true;
            ai = new DummyAi();
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 이 메소드는 항상 바운드된 게임 스레드에서 실행된다.
        /// </remarks>
        /// <returns></returns>
        public IngameEvent[] ProcessTurn()
        {
            return ai.Process();
        }

        #region FAKE_SESSION
        public override bool isAlive
        {
            get
            {
                return true;
            }
        }
        internal override void SendRawPacket(string packet)
        {
            // DoNothing
        }
        internal override void SendPacket(PacketBase packet)
        {
            // DoNothing
            // 페이크 세션이기 때문에 실제로 보내면 안됨
        }
        #endregion
    }
}
