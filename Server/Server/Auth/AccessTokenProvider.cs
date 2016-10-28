using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Auth
{
    class AccessTokenProvider
    {
        private static IAccessTokenManager manager { get; set; }

        static AccessTokenProvider()
        {
            manager = new AccessTokenManagerSimple();
        }

        public static Task<string> CreateNew(
            string userType, string userId, string accessToken)
        {
            return manager.CreateNew(userType, userId, accessToken);
        }

        public static Task<string> Query(string accessToken)
        {
            return manager.Query(accessToken);
        }
    }
}
