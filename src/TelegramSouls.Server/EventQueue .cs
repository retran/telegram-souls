using System.Collections.Concurrent;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server
{
    public class EventQueue
    {
        private ConcurrentQueue<Event> _queue;

        public long Turn = 0;

        public EventQueue()
        {
            _queue = new ConcurrentQueue<Event>();
        }

        public Event Peek()
        {
            Event e;
            if (_queue.TryPeek(out e))
            {
                return e;
            }
            return null;
        }

        public Event Dequeue()
        {
            Event e;
            if (_queue.TryDequeue(out e))
            {
                return e;
            }
            return null;
        }

        public void Enqueue(Event e)
        {
            _queue.Enqueue(e);
        }
    }
}
