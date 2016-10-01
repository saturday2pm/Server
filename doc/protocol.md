```cs
class Player {
}

class Waypoint {
}
```

```cs
class Join {
  
}
```

```cs
class IngamePacket {
  public DateTime timestamp {get;set;}
}

// 게임이 시작하면 (큐가 잡히면)
// 서버가 해당 룸의 모든 플레이어에게 전달하는 패킷
//
// * timestamp : 게임이 시작한 서버 타임
class StartGame : IngamePacket {
  public Player[] players {get;set;}
  public long seed {get;set;}

  public int mapId {get;set;}
}

class Move : IngamePacket {
  public Waypoint from {get;set;}
  public Waypoint to {get;set;}
}


```
