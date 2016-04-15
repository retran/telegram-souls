using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server
{
    class User
    {
        public long Id;
        public string Username;
    }

    public class MessageHandler
    {
        private Client _client;
        private MessageQueue _queue;
        private CancellationTokenSource _cancellationTokenSource;

        private ConcurrentDictionary<long, User> _sessions;

        public MessageHandler(Client client, MessageQueue queue)
        {
            _client = client;
            _queue = queue;
            _sessions = new ConcurrentDictionary<long, User>();
        }

        private void Broadcast(long userId, string text)
        {
            foreach (var user in _sessions.Values.Where(i => i.Id != userId))
            {
                _client.SendMessage(new SendMessageQuery()
                {
                    ChatId = user.Id,
                    Text = text
                });
            }
        }

        private void Handle()
        {
            var message = _queue.Dequeue();
            if (message != null)
            {
                if (string.Equals(message.Text, "/start", System.StringComparison.OrdinalIgnoreCase))
                {
                    if (!_sessions.ContainsKey(message.UserId))
                    {
                        _sessions.TryAdd(message.UserId, new User()
                        {
                            Id = message.UserId,
                            Username = message.Username
                        });
                    }

                    Broadcast(message.UserId, string.Format("Такой-то хер {0} вошел, громко хлопнув дверью.", message.Username));

                    return;
                }

                if (!_sessions.ContainsKey(message.UserId))
                {
                    return;
                }

                if (string.Equals(message.Text, "/stop", System.StringComparison.OrdinalIgnoreCase))
                {
                    User user;
                    _sessions.TryRemove(message.UserId, out user);
                }

                if (string.Equals(message.Text, "/who", System.StringComparison.OrdinalIgnoreCase))
                {
                    var list = string.Join(", ", _sessions.Values.Select(v => v.Username));
                    _client.SendMessage(new SendMessageQuery()
                    {
                        ChatId = message.UserId,
                        Text = list,
                        ReplyToMessageId = message.EventId
                    });

                    return;
                }

                Broadcast(message.UserId, string.Format("{0}: {1}", message.Username, message.Text));
            }
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            Task.Factory.StartNew(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Handle();
                }
            });
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}