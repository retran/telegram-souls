using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TelegramSouls.Server.Telegram;
using System;

namespace TelegramSouls.Server
{
    class User
    {
        public long Id;
        public string Username;
    }

    public class MessageHandler : IDisposable 
    {
        private TelegramClient _client;
        private MessageQueue _queue;
        private CancellationTokenSource _cancellationTokenSource;

        public MessageHandler(TelegramClient client, MessageQueue queue,
            SessionStorage sessions)
        {
            _client = client;
            _queue = queue;
            _sessions = sessions;
        }

        private void Broadcast(long userId, string text)
        {
            foreach (var user in _sessions.GetSessions().Where(i => i.Id != userId))
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
                    if (!_sessions.IsSessionActive(message.From.Id))
                    {
                        _sessions.Create(message.From.Id, message.From.Username);
                    }

                    Broadcast(message.From.Id, string.Format("Такой-то хер {0} вошел, громко хлопнув дверью.", message.From.Username));

                    return;
                }

                if (!_sessions.IsSessionActive(message.From.Id))
                {
                    return;
                }

                if (string.Equals(message.Text, "/stop", System.StringComparison.OrdinalIgnoreCase))
                {
                    _sessions.Abandon(message.From.Id);
                }

                if (string.Equals(message.Text, "/who", System.StringComparison.OrdinalIgnoreCase))
                {
                    var list = string.Join(", ", _sessions.GetSessions().Select(v => v.Username));
                    _client.SendMessage(new SendMessageQuery()
                    {
                        ChatId = message.From.Id,
                        Text = list,
                        ReplyToMessageId = message.MessageId
                    });

                    return;
                }

                Broadcast(message.From.Id, string.Format("{0}: {1}", message.From, message.Text));
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

        private bool _disposed = false;
        private SessionStorage _sessions;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // ...
                }

                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _disposed = true;
            }
        }

        ~MessageHandler()
        {
            Dispose(false);
        }
    }
}