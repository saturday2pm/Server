```cs
class Player {
}

class Waypoint {
}
```

```cs
class IngamePacket {
  public DateTime timestamp {get;set;}
}

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
