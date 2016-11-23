using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GSF;
using GSF.Concurrency;

namespace Server.LoadBalancing
{
    /// <summary>
    /// 이 인터페이스를 상속받아 로드밸런서를 구현한다.
    /// </summary>
    interface ILoadBalancer
    {
        /// <summary>
        /// 초기화.
        /// 초기부터 가지고 시작하는 호스트 목록이 인자로 들어옴
        /// </summary>
        /// <param name="hosts">서버 호스트 목록</param>
        void Initialize(string[] hosts);

        /// <summary>
        /// 다음번에 접속할 호스트 주소를 가져온다.
        /// </summary>
        /// <returns>서버 호스트 주소</returns>
        [ThreadSafe]
        string GetNext();
    }
}
