﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Auth
{
    using IDP;

    class AuthHandler
    {
        private static Local local = new Local();
        private static Facebook facebook = new Facebook();

        public static async Task<bool> Login(
            string userType,
            string userId, string accessToken)
        {
            /* TODO : 이곳에 IDP 핸들러들을 추가한다 */

            // 게스트는 별도의 accessToken을 가지고 있지 않으며
            // 항상 성공한다.
            if (userType == UserType.Guest)
                return true;

            if (userType == UserType.Local)
                return await local.IsValidToken(userId, accessToken);

            if (userType == UserType.Facebook)
                return await facebook.IsValidToken(userId, accessToken);

            throw new NotSupportedException("unknown userType");
        }
    }
}
