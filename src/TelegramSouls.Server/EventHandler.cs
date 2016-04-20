using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramSouls.Server
{
    public class EventHandler : IDisposable
    {
        private SessionStorage _sessions;
        private MessageSender _sender;
        private EventQueue _queue;

        private bool _disposed = false;
        private CancellationTokenSource _cancellationTokenSource;

        public EventHandler(MessageSender sender, EventQueue queue,
            SessionStorage sessions)
        {
            _sender = sender;
            _queue = queue;
            _sessions = sessions;
        }

        private void Handle()
        {
            var e = _queue.Dequeue();
            e.Handle();
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            Task.Factory.StartNew(() =>
            {
                var dt = DateTime.Now;
                while (!cancellationToken.IsCancellationRequested)
                {
                    if ((DateTime.Now - dt).Seconds > 1)
                    {
                        dt = DateTime.Now;
                        _queue.Turn++;
                        if (_queue.Peek() != null)
                        {
                            Handle();
                        }
                    }
                }
            });
        }

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

        ~EventHandler()
        {
            Dispose(false);
        }
    }
}
