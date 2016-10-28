using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Auth
{
    class AccessTokenManagerSimple : IAccessTokenManager
    {
        private Dictionary<string, string> storage { get; set; }

        public AccessTokenManagerSimple()
        {
            storage = new Dictionary<string, string>();
        }

        public Task<string> CreateNew(string userType, string userId, string accessToken)
        {
            var token = Guid.NewGuid().ToString();
            storage[token] = userId;

            return Task.FromResult(token);
        }

        public Task<string> Query(string accessToken)
        {
            if (storage.ContainsKey(accessToken) == false)
                return Task.FromResult<string>(null);

            return Task.FromResult(storage[accessToken]);
        }
    }
}
