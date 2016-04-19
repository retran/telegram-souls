using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSouls.Server
{
    public class SessionStorage
    {
        private ConcurrentDictionary<long, Session> _sessions;

        public SessionStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }

        public Session Create(long id, string username)
        {
            var session = _sessions.GetOrAdd(id, userId => // TODO use factory to inject persiting service
            {
                var s = new Session(id, username);
                s.Load();
                return s;
            });

            return session;
        }

        public bool IsSessionActive(long id)
        {
            return _sessions.ContainsKey(id);
        }

        public IEnumerable<Session> GetSessions()
        {
            return _sessions.Values;
        }

        public void Abandon(long id)
        {
            Session session;
            _sessions.TryRemove(id, out session);
            session.Persist();
        }
    }
}
