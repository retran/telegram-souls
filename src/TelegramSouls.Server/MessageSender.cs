using System.Linq;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server
{
    public class MessageSender
    {
        private TelegramClient _client;
        private SessionStorage _sessions;

        public MessageSender(TelegramClient client, SessionStorage sessions)
        {
            _client = client;
            _sessions = sessions;
        }

        public void Broadcast(SessionContext context, string text)
        {
            foreach (var session in _sessions.GetSessions().Where(i => i.Id != context.Id))
            {
                _client.SendMessage(new SendMessageQuery()
                {
                    ChatId = session.Id,
                    Text = text
                });
            }
        }

        public void SendToRoom(SessionContext context, string text)
        {
            foreach (var session in _sessions.GetSessions().Where(i => i.Id != context.Id && string.Equals(i.Room, context.Room)))
            {
                _client.SendMessage(new SendMessageQuery()
                {
                    ChatId = session.Id,
                    Text = text
                });
            }
        }

        public void ReplyTo(SessionContext context, long messageId, string text)
        {
            _client.SendMessage(new SendMessageQuery()
            {
                ChatId = context.Id,
                Text = text,
                ReplyToMessageId = messageId
            });
        }

        public void SendTo(SessionContext context, string text)
        {
            _client.SendMessage(new SendMessageQuery()
            {
                ChatId = context.Id,
                Text = text
            });
        }
    }
}
