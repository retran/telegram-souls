using Autofac;
using TelegramSouls.Server.World;

namespace TelegramSouls.Server
{
    public class SessionContextFactory
    {
        private ILifetimeScope _scope;
        private MessageSender _sender;

        public SessionContextFactory(ILifetimeScope scope, MessageSender sender)
        {
            _sender = sender;
            _scope = scope;
        }

        public SessionContext Create(long id, string username)
        {
            var area = _scope.Resolve<Area>();

            return new SessionContext(_sender, area, id, username);
        }
    }
}
