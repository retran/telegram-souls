using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TelegramSouls.Server.Telegram;
using System;

namespace TelegramSouls.Server
{
    public class MessageHandler : IDisposable 
    {
        private TelegramClient _client;
        private MessageQueue _queue;
        private CancellationTokenSource _cancellationTokenSource;

        public MessageHandler(MessageSender sender, MessageQueue queue,
            SessionStorage sessions)
        {
            _sender = sender;
            _queue = queue;
            _sessions = sessions;
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
                        var context = _sessions.Create(message.From.Id, message.From.Username);
                        _sender.SendToRoom(context, string.Format("Такой-то хер {0} вошел, громко хлопнув дверью.", message.From.Username));
                    }

                    return;
                }

                if (!_sessions.IsSessionActive(message.From.Id))
                {
                    return;
                }

                var sessionContext = _sessions.Get(message.From.Id);

                if (string.Equals(message.Text, "/stop", System.StringComparison.OrdinalIgnoreCase))
                {
                    _sender.SendToRoom(sessionContext, string.Format("Такой-то хер {0} безвременно покинул нас.", message.From.Username));
                    _sessions.Abandon(sessionContext.Id);
                }

                if (string.Equals(message.Text, "/who", System.StringComparison.OrdinalIgnoreCase))
                {
                    var list = string.Join(", ", _sessions.GetSessions().Select(v => v.Username));
                    _sender.ReplyTo(sessionContext, message.MessageId, list);                    
                    return;
                }

                _sender.SendToRoom(sessionContext, string.Format("{0}: {1}", sessionContext.Username, message.Text));
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
        private MessageSender _sender;

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