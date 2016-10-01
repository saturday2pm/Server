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

클라이언트와 서버는 서로 동기화되는 프레임레이트를 가진다.<br>

클라이언트는 매 프레임마다 발생한 이벤트의 묶음을 서버로 전송한다.<br>
서버는 게임룸의 모든 클라이언트에서 수신한 이벤트들을 종합해서 다시 되돌려준다.
