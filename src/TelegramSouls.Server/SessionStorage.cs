using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TelegramSouls.Server.Telegram;
using TelegramSouls.Server.World;

namespace TelegramSouls.Server
{
    public class SessionStorage
    {
        private ConcurrentDictionary<long, SessionContext> _sessions;
        private Area _area;
        private MessageSender _sender;
        private SessionContextFactory _sessionContextFactory;

        public SessionStorage(SessionContextFactory sessionContextFactory)
        {
            _sessions = new ConcurrentDictionary<long, SessionContext>();
            _sessionContextFactory = sessionContextFactory;
        }

        public SessionContext Create(long id, string username)
        {
            var session = _sessions.GetOrAdd(id, userId => // TODO use factory to inject persiting service
            {
                var s = _sessionContextFactory.Create(userId, username);
                s.Load();
                return s;
            });

            return session;
        }

        public SessionContext Get(Message message)
        {
            SessionContext session;

            if (!_sessions.TryGetValue(message.From.Id, out session))
            {
                throw new InvalidOperationException();
            }

            return session;
        }

        public bool IsSessionActive(long id)
        {
            return _sessions.ContainsKey(id);
        }

        public IEnumerable<SessionContext> GetSessions()
        {
            return _sessions.Values;
        }

        public void Abandon(long id)
        {
            SessionContext session;
            _sessions.TryRemove(id, out session);
            session.Persist();
        }
    }
}
