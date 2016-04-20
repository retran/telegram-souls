using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server
{
    public class MessagePoller : IDisposable
    {
        private TelegramClient _client;
        private MessageQueue _queue;
        private long _offset = 0;
        private CancellationTokenSource _cancellationTokenSource;

        public MessagePoller(TelegramClient client, MessageQueue queue)
        {
            _client = client;
            _queue = queue;
        }

        private async void Poll()
        {
            var updates = await _client.GetUpdates(new GetUpdatesQuery()
            {
                Offset = _offset
            });

            if (updates.Result.Any())
            {
                _offset = updates.Result.Last().UpdateId + 1;
            }

            foreach (var update in updates.Result)
            {
                if (string.Equals(update.Message.Chat.Type, "private", System.StringComparison.OrdinalIgnoreCase))
                {
                    _queue.Enqueue(update.Message);
                    Trace.WriteLine(string.Format("{0}\t{1}\t{2}", DateTime.Now, update.Message.From.Username, update.Message.Text));
                }
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
                    Poll();
                }
            });
        }

        private bool _disposed = false;

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

        ~MessagePoller()
        {
            Dispose(false);
        }
    }
}
