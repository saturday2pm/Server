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

서버 종류
----
