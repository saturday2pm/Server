matchmaking
====

flow
----
```cs
var wsMatch = new WebSocket();
var wsGame = new WebSocket();

wsMatch.JoinQueue();

// 매치 서버가 플레이어를 찾아줬음
if (p = wsMatch.MatchSuccess()) {
  wsGame.Connect(p.gameServerAddress);

  wsGame.JoinGame(
    matchToken: p.matchToken);

  // 매치가 성공함
  if (p = wsGame.StartGame()) {

  }
  // 매치 취소됨
  else if (p = wsGame.CancelGame()) {

  }
}

```