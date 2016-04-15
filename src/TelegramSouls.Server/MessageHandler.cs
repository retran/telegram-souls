using System.Threading;
using System.Threading.Tasks;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server
{
    public class MessageHandler
    {
        private Client _client;
        private MessageQueue _queue;
        private CancellationTokenSource _cancellationTokenSource;

        public MessageHandler(Client client, MessageQueue queue)
        {
            _client = client;
            _queue = queue;
        }

        private void Handle()
        {
            var message = _queue.Dequeue();
            if (message != null)
            {
                _client.SendMessage(new SendMessageQuery()
                {
                    ChatId = message.UserId,
                    ReplyToMessageId = message.EventId,
                    Text = message.Text
                });
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