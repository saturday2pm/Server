```cs
class Player {
  public string name {get;set;}
}

// 경유지가 될 수 있는 모든 곳
// 주로 성
class Waypoint {
  // 좌표는 맵이 가지고 있으므로 패킷에서는 아이디로만 관리한다.
  public int id {get;set;}
}
class Castle : Waypoint {
  // 업그레이드 정도 (1, 2, 3...)
  public int level {get;set;}
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
  public Player player {get;set;}
}

// 게임이 시작하면 (큐가 잡히면)
// 서버가 해당 룸의 모든 플레이어에게 전달하는 패킷
class StartGame : IngamePacket {
  public Player[] players {get;set;}
  public long seed {get;set;}

  public int mapId {get;set;}
}
class RejoinGame : StartGame {
  public Frame[] frames {get;set;}
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

class Upgrade : IngameEvent {
  public Castle castle {get;set;}
  
}
```

클라이언트와 서버는 서로 동기화되는 프레임레이트를 가진다.<br>

클라이언트는 매 프레임마다 발생한 이벤트의 묶음을 서버로 전송한다.<br>
서버는 게임룸의 모든 클라이언트에서 수신한 이벤트들을 종합해서 다시 되돌려준다.

* 서버가 클라로부터 제시간에 `Frame` 패킷을 받지 못하면,
  * 해당 프레임의 처리를 지연한다. (클라에게 발송 안함)
  * 한놈이 계속 안보내면 짜름
* 클라가 서버로부터 제시간에 `Frame` 패킷을 받지 못하면, 
  * `동기화중...` 메세지를 출력하고 모든 입력과 이벤트를 멈춘다.
  * 계속 안오면, 리조인 처리



* 모든 이벤트는 선실행 하지 않고, 서버에서 `Frame` 패킷이 오면 실행한다.
  * 자기가 어디로 드래그해서 이동 시켜도, 이동 시키지 않고 다음 프레임에 서버에서 `Frame` 패킷이 오면 그거 보고 처리함.
    * 롤급의 빠른 반응성의 게임이 아니라 별로 안답답한수준이아니라 걍 안답답할듯
    * 아니면 서버 응답 받기 전에 미리 유닛을 스폰만 해놓으셔도 됩니다.
    * 자기가 이동시킨 병력은 성의 중간부터 / 다른사람은 성의 중간에서 좀 더 멀리부터 스폰되도록
  * 클라의 모든 로직을 서버 의존적으로 만들 수 있음
    * 자기것만 특수처리 안해도 됨
    * 모든 플레이어의 처리 동일하게 한곳에서 함
