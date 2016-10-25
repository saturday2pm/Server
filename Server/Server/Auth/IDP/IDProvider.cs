using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Auth.IDP
{
    interface IDProvider
    {
        Task<bool> IsValidToken(string userId, string accessToken);
    }
}
