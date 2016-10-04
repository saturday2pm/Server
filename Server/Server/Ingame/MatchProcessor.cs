using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ProtocolCS.Constants;

namespace Server.Ingame
{
    using AI;
    using MatchMaking;

    class MatchProcessor
    {
        public Match match { get; private set; }

        public IngameService[] players { get; private set; }
        private int _joinedPlayerCount;
        public int joinedPlayerCount => _joinedPlayerCount;

        public MatchState matchState { get; set; }

        public MatchProcessor(Match match)
        {
            this.match = match;
            this.players = new IngameService[match.playerIds.Length];

            this.matchState = MatchState.Ready;

            // 시작부터 함께한 봇 유저 처리
            for (int i=0;i<match.playerIds.Length;i++)
            {
                if (match.playerIds[i] != ReservedPlayerId.Bot)
                    continue;

                players[i] = AutoPlayer.Create();
                Interlocked.Increment(ref _joinedPlayerCount);
            }
        }

        private int GetIndex(int playerId)
        {
            var idx = -1;
            for (int i = 0; i < match.playerIds.Length; i++)
            {
                if (match.playerIds[i] == playerId)
                    idx = i;
            }

            // 아마도 허용된 플레이어 아닌사람이 이방에 진입한 케이스
            if (idx == -1)
                throw new InvalidOperationException("");

            return idx;
        }
        public bool Join(IngameService playerSession)
        {
            if (matchState != MatchState.Ready)
                throw new InvalidOperationException("MatchState != .Ready");

            var idx = GetIndex(playerSession.currentPlayerId);

            players[idx] = playerSession;

            if (Interlocked.Increment(ref _joinedPlayerCount)
                == players.Length)
                return true;
            return false;
        }

        public bool CanStartGame()
        {
            // 그사이에 나간사람 없는지 검사
            foreach(var player in players)
            {
                if (player.isAlive == false)
                    return false;
            }

            // 접속 플레이어가 방 전체 인원과 맞는지 검사
            return joinedPlayerCount == players.Length;
        }

        public GameProcessor Start()
        {
            if (matchState != MatchState.Ready)
                throw new InvalidOperationException("matchState != .Ready");

            var gameProcessor = new GameProcessor(players);

            matchState = MatchState.Started;

            return gameProcessor;
        }
        public void Cancel()
        {
            if (matchState != MatchState.Ready)
                throw new InvalidOperationException("matchState != .Ready");

            matchState = MatchState.Canceled;
        }
    }
}
