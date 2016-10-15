# Server
Server

서버에 연결하기
----
서버 주소는 __호스트__ + __서비스__ + __페이로드__의 구조를가진다.
```
ws://riniblog.egloos.com/game?version=1.0.0
```
* __호스트__ : riniblog.egloos.com
* __서비스__ : /game
* __페이로드__ : ?version=1.0.0
  * 클라이언트에서는 반드시 version 값을 지정해서 연결해야 하며, 불일치 시 서버가 연결을 거부할 수 있다.

<br>
__헬퍼 함수 이용하기__<br>
```cs
using ProtocolCS;

string uri = UriBuilder.Create(
  "ws://riniblog.me/game",
  PLAYER_ID, ACCESS_TOKEN);
```

서버 종류
----

유저 아이디
----
비로그인 기반 인증으로, 클라이언트가 보낸 유저아이디를 무조건 신뢰함<br>
클라이언트는 자기 시스템에 고유한 아이디를 유저 아이디로 사용한다.
```
var playerId = SystemInfo.deviceUniqueIdentifier.GetHashCode();
```
추후에 패북 로그인같은걸 붓이면 얘가 이상한 유저 아이디인지 진짜인지 검증 가능함
