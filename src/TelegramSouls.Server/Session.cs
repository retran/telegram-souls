using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSouls.Server
{
    public class Session
    {
        public long Id { get; private set; }
        public string Username { get; private set; }
        public string RoomID { get; set; }

        public Session(long id, string username)
        {
            Id = id;
            Username = username;
        }

        public void Load()
        {
            //
        }

        public void Persist()
        {
            //
        }
    }
}
