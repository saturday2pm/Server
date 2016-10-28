유저 인증
====
서버 접속 URI에 유저 정보를 넛으면 알아서 인증되니다.<br>
인증 안되면 웹소켓 에러를 내면서 연결을 끊습니다.<br>

```
// 다른 query string는 생략합니다. (버전 등)
ws://localhost?userType=&userId=&accessToken
```

guest
----
항상 성공합니다. accessToken은 체크하지 않으며, 무조건 클라가 준 userId를 믿습니다. 테스트용으로 쓰세요.

local
----
이쪽 서버가 발급한 accessToken을 사용합니다.<br>
이서버가 엑세스 토큰을 또주는 이유는, 재접속/또는 게임서버에 접속할때 `userType`을 __facebook__으로 넛으면 서버는 페북서버에다가 토큰이 맞는지 또물어봐야됨.<br>
근대 한번 페북으로 ㅇㅋ 해주고 이쪽에서 토큰 만들어주면 안가도됨.

facebook
----
머볼건
