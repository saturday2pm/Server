using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolCS;
using WebSocketSharp;

using GSF;

namespace Server.Ingame
{
    partial class IngameService : Service<IngameService>, ICheckable
    {
        public static readonly string Path = "/game";

        private IngameSessionState sessionState { get; set; }
        public bool isBotPlayer { get; set; }
        
        static IngameService()
        {
            // IngameService.Match.cs
            InitMatch();
        }

        public IngameService()
            : base()
        {
            sessionState = IngameSessionState.Connected;
        }

        public Player AsPlayer()
        {
            return new Player
            {
                id = UserId,
                name = UserId.ToString()
            };
        }

        public bool OnHealthCheck()
        {
            var elapsed = StartTime - DateTime.Now;

            if (sessionState == IngameSessionState.Playing)
            {
                // 게임 중이어도 1시간 이내 접속중인것만 인정
                if (elapsed < TimeSpan.FromHours(1))
                    return true;
                // 게임 중인데, 1시간 이상 접속함 -> 비정상
                else
                    return false;
            }

            // 접속한지 30초 지났나?
            if (elapsed < TimeSpan.FromSeconds(30))
                return true;

            // Playing 상태 아니고 / 30초 이상 지난 세션은 다 닫음
            return false;
        }

        protected override void OnClose(CloseEventArgs e)
        {
            if (gameProcessor == null)
                return;

            // 나간 플레이어는 봇으로 대체한다.
            gameProcessor.ToAutoPlayer(UserId);

            if (gameProcessor.isZombieGame)
            {
                // 좀비 방 -> 폭파
            }
        }
    }
}
