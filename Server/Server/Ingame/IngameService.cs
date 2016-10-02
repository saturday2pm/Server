using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;
using WebSocketSharp;

namespace Server.Ingame
{
    public partial class IngameService : Service<IngameService>
    {
        public static readonly string Path = "/game";

        public bool isBotPlayer { get; set; }
        
        static IngameService()
        {
            // IngameService.Match.cs
            InitMatch();
        }

        public Player AsPlayer()
        {
            return new Player
            {
                id = currentPlayerId,
                name = currentPlayerId.ToString()
            };
        }

        protected override void OnClose(CloseEventArgs e)
        {
            if (gameProcessor == null)
                return;

            // 나간 플레이어는 봇으로 대체한다.
            gameProcessor.ToAutoPlayer(currentPlayerId);

            if (gameProcessor.isZombieGame)
            {
                // 좀비 방 -> 폭파
            }
        }
    }
}
