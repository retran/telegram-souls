using Autofac;
using System.Linq;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server
{
    public class MessageSender
    {
        private TelegramClient _client;
        private ILifetimeScope _scope;

        public MessageSender(ILifetimeScope scope, TelegramClient client)
        {
            _client = client;
            _scope = scope;
        }

        public void Broadcast(SessionContext context, string text)
        {
            var sessions = _scope.Resolve<SessionStorage>().GetSessions();
            foreach (var session in sessions.Where(i => i.Id != context.Id))
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
            var sessions = _scope.Resolve<SessionStorage>().GetSessions();
            foreach (var session in sessions.Where(i => i.Id != context.Id && string.Equals(i.RoomId, context.RoomId)))
            {
                _client.SendMessage(new SendMessageQuery()
                {
                    ChatId = session.Id,
                    Text = text,
                    ReplyMarkup = new ReplyKeyboardMarkup()
                    {
                        Keyboard = session.GetKeyboard(),
                        ResizeKeyboard = true
                    }
                });
            }
        }

        public void ReplyTo(SessionContext context, string text)
        {
            _client.SendMessage(new SendMessageQuery()
            {
                ChatId = context.Id,
                Text = text,
                ReplyToMessageId = context.ReplyId,
                ReplyMarkup = new ReplyKeyboardMarkup()
                {
                    Keyboard = context.GetKeyboard(),
                    ResizeKeyboard = true
                }
            });
        }

        public void SendTo(SessionContext context, string text)
        {
            _client.SendMessage(new SendMessageQuery()
            {
                ChatId = context.Id,
                Text = text,
                ReplyMarkup = new ReplyKeyboardMarkup()
                {
                    Keyboard = context.GetKeyboard(),
                    ResizeKeyboard = true
                }
            });
        }
    }
}
