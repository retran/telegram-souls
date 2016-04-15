using System;

namespace TelegramSouls.Server
{
    public class Message
    {
        public long EventId;
        public long UserId;
        public string Text;
        public string Username;
        public DateTime TimeStamp;
    }
}