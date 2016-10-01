```cs
class Player {
  public string name {get;set;}
}

class Waypoint {
  public int id {get;set;}
}
```

```cs
class Join {
  
}
```

```cs
class IngamePacket {
  public int frameNo {get;set;}
}
class IngameEvent {
}

// 게임이 시작하면 (큐가 잡히면)
// 서버가 해당 룸의 모든 플레이어에게 전달하는 패킷
class StartGame : IngamePacket {
  public Player[] players {get;set;}
  public long seed {get;set;}

  public int mapId {get;set;}
}
class Frame : IngamePacket {
  public IngameEvent[] events {get;set;}
}
```

__Ingame Events__
```cs
class Move : IngameEvent {
  public Waypoint from {get;set;}
  public Waypoint to {get;set;}
}


```
