using System.Collections.Concurrent;

namespace TelegramSouls.Server
{
    public class MessageQueue
    {
        private ConcurrentQueue<Message> _queue;

        public MessageQueue()
        {
            _queue = new ConcurrentQueue<Message>();
        }

        public Message Peek()
        {
            Message e;
            if (_queue.TryPeek(out e))
            {
                return e;
            }
            return null;
        }

        public Message Dequeue()
        {
            Message e;
            if (_queue.TryDequeue(out e))
            {
                return e;
            }
            return null;
        }

        public void Enqueue(Message e)
        {
            _queue.Enqueue(e);
        }
    }
}
