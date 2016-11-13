﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using WebSocketSharp;
using WebSocketSharp.Server;

using ProtocolCS;

namespace Server
{
    using Auth;

    public partial class Service<T> : WebSocketBehavior, ICheckable
    {
        private static Dictionary<Type, MethodInfo> handlers { get; set; }

        private static ConcurrentDictionary<int, T> sessions { get; set; }

        private int _currentPlayerId;
        public int currentPlayerId
        {
            get { return _currentPlayerId; }
            set
            {
                sessions[value] = (T)(object)this;
                _currentPlayerId = value;
            }
        }

        public virtual bool isAlive
        {
            get
            {
                return State == WebSocketState.Open;
            }
        }

        static Service()
        {
            handlers = new Dictionary<Type, MethodInfo>();
            sessions = new ConcurrentDictionary<int, T>();

            var handlerCandidates = typeof(T).GetMethods(
                BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetParameters().Length == 1)
                .Where(x => x.GetParameters().First().ParameterType.Namespace == nameof(ProtocolCS))
                .Where(x => x.ReturnType == typeof(void));

            foreach (var candidate in handlerCandidates)
            {
                var packetType = candidate
                    .GetParameters().First()
                    .ParameterType;

                handlers[packetType] = candidate;
            }
        }

        /// <summary>
        /// 숫자로된 플레이어 아이디로 세션을 가져온다.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>세션</returns>
        /// <exception cref="KeyNotFoundException">
        /// 주어진 playerId로 찾을 수 없을 때
        /// </exception>
        [ThreadSafe]
        protected static T GetSessionById(int playerId)
        {
            return sessions[playerId];
        }

        protected void ErrorClose(CloseStatusCode code, string reason)
        {
            if (State != WebSocketState.Open)
            {
                Console.WriteLine("State != Open");
                return;
            }

            Sessions.CloseSession(this.ID, code, reason);
        }

        internal virtual void SendRawPacket(string packet)
        {
            try
            {
                Send(packet);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        internal virtual void SendPacket(PacketBase packet)
        {
            try
            {
                var json = Serializer.ToJson(packet);

                SendRawPacket(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task ProcessLogin(
            string userType, string userId, string accessToken)
        {
            Console.WriteLine($"ProcessLogin({userType}, {userId}, {accessToken}");

            try
            {
                if (string.IsNullOrEmpty(userType))
                    throw new ArgumentException(nameof(userType));
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException(nameof(userId));

                var loggedIn = await AuthHandler.Login(
                    userType, userId, accessToken);

                if (loggedIn == false)
                    throw new InvalidOperationException();

                currentPlayerId = int.Parse(userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorClose(CloseStatusCode.InvalidData, "login error");
            }
        }

        /// <summary>
        /// 각각의 서비스들에서 이 메소드를 상속받아 검사를 수행한다.
        /// </summary>
        /// <returns>false일 경우 세션 종료</returns>
        public virtual bool OnHealthCheck()
        {
            return true;
        }
        public void OnDispose()
        {
            ErrorClose(CloseStatusCode.ServerError, "healthcheck failure");
        }

        protected override async void OnOpen()
        {
            // 클라이언트 버전 체크
            #region CHECK_VERSION
            Version clientVersion = null;
            if (Version.TryParse(
                Context.QueryString.Get("version"),
                out clientVersion) == false)
            {
                ErrorClose(CloseStatusCode.InvalidData, "invalid version string");
                return;
            }

            if (ProtocolCS.Constants.ProtocolVersion.version
                != clientVersion)
            {
                ErrorClose(CloseStatusCode.ProtocolError, "serverVersion != clientVersion");
                return;
            }
            #endregion

            // 로그인
            #region LOGIN
            var userType = Context.QueryString.Get("userType");
            var userId = Context.QueryString.Get("userId");
            var accessToken = Context.QueryString.Get("accessToken");

            await ProcessLogin(userType, userId, accessToken);
            #endregion

            HealthChecker.Add(this);
        }
        protected override void OnClose(CloseEventArgs e)
        {
            T trash;
            sessions.TryRemove(currentPlayerId, out trash);

            HealthChecker.Remove(this);
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            var json = e.Data;
            object packet = null;

            if (e.IsText == false)
            {
                ErrorClose(CloseStatusCode.ProtocolError, "only json data accepted");
                return;
            }
            
            try
            {
                packet = Serializer.ToObject(json);
            }
            catch (Exception ex)
            {
            }

            if (packet == null)
            {
                Console.WriteLine($"Parsing Error : {e.Data}");
                ErrorClose(CloseStatusCode.InvalidData, "parsing error");
            }
            else if (handlers.ContainsKey(packet.GetType()) == false)
            {
                Console.WriteLine($"Unkown Packet : {e.Data}");
                ErrorClose(CloseStatusCode.InvalidData, "unknown packet");
            }
            else
            {
                var handler = handlers[packet.GetType()];

                try
                {
                    handler.Invoke(this, new object[] { packet });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ErrorClose(CloseStatusCode.ServerError, "internal server error");
                }
            }
        }
    }
}
