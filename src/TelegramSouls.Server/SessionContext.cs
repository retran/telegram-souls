using System;
using TelegramSouls.Server.Telegram;
using TelegramSouls.Server.World;

namespace TelegramSouls.Server
{
    public class SessionContext
    {
        private Area _area;
        private object _syncRoot = new object();
        private long _replyId;
        private MessageSender _sender;

        public long Id { get; private set; }
        public string Username { get; private set; }
        public string RoomId { get; set; }
        public bool InGame { get; set; }

        public long ReplyId
        {
            get
            {
                lock (_syncRoot)
                {
                    return _replyId;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _replyId = value;
                }
            }
        }

        public SessionContext(MessageSender sender, Area area, long id, string username)
        {
            Id = id;
            Username = username;
            _area = area;
            _sender = sender;
        }

        public void Load()
        {
            RoomId = "Entrance";
            InGame = false;
        }

        public void Persist()
        {
        }

        public KeyboardButton[][] GetKeyboard()
        {
            return new KeyboardButton[][]
            {
                _area.GetRoom(RoomId).GetContextActions(this),
                _area.GetRoom(RoomId).GetNavigationalButtons(this)
            };
        }

        public void Broadcast(string text)
        {
            _sender.Broadcast(this, text);
        }

        public void SendToRoom(string text)
        {
            _sender.SendToRoom(this, text);
        }

        public void Reply(string text)
        {
            _sender.ReplyTo(this, text);
        }

        public void Send(string text)
        {
            _sender.SendTo(this, text);
        }

        public Room GetRoom()
        {
            return _area.GetRoom(RoomId);
        }
    }
}
