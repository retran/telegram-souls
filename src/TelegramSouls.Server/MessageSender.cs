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
                ReplyToMessageId = messageId,
                ReplyMarkup = new ReplyKeyboardMarkup()
                {
                    Keyboard = new KeyboardButton[][]
                        {
                            new KeyboardButton[]
                            {
                                new KeyboardButton()
                                {
                                    Text = "|Осмотреть|"
                                },
                                new KeyboardButton()
                                {
                                    Text = "|Взять|"
                                },
                                new KeyboardButton()
                                {
                                    Text = "|Атаковать|"
                                }
                            },
                            new KeyboardButton[]
                            {
                                new KeyboardButton()
                                {
                                    Text = "|Север|"
                                },
                                new KeyboardButton()
                                {
                                    Text = "|Восток|"                                
                                },
                                new KeyboardButton()
                                {
                                    Text = "|Юг|"
                                },
                                new KeyboardButton()
                                {
                                    Text = "|Запад|"
                                },
                                new KeyboardButton()
                                {
                                    Text = "|Вверх|"
                                },
                                new KeyboardButton()
                                {
                                    Text = "|Вниз|"
                                }

                            }
                        },
                    ResizeKeyboard = true
                }
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
