using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TelegramSouls.Server
{
    public class SessionStorage
    {
        private ConcurrentDictionary<long, SessionContext> _sessions;

        public SessionStorage()
        {
            _sessions = new ConcurrentDictionary<long, SessionContext>();
        }

        public SessionContext Create(long id, string username)
        {
            var session = _sessions.GetOrAdd(id, userId => // TODO use factory to inject persiting service
            {
                var s = new SessionContext(id, username);
                s.Load();
                return s;
            });

            return session;
        }

        public SessionContext Get(long id)
        {
            SessionContext session;

            if (!_sessions.TryGetValue(id, out session))
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
