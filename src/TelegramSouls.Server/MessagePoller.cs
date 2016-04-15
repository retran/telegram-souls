using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server
{
    public class MessagePoller
    {
        private Client _client;
        private MessageQueue _queue;
        private long _offset = 0;
        private CancellationTokenSource _cancellationTokenSource;

        public MessagePoller(Client client, MessageQueue queue)
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
                    _queue.Enqueue(new Message()
                    {
                        UserId = update.Message.Chat.Id,
                        EventId = update.Message.MessageId,
                        Text = update.Message.Text,
                        TimeStamp = DateTime.Now,
                        Username = update.Message.From.Username
                    });
                    Console.WriteLine("{0} {1}: {2}", DateTime.Now, update.Message.From.Username, update.Message.Text);
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

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
