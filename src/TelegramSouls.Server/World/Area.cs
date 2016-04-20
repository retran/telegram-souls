using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSouls.Server.World
{
    public class Area
    {
        private ConcurrentDictionary<string, Room> _rooms;

        public Area(EventQueue eventQueue, SessionStorage sessions)
        {
            _rooms = new ConcurrentDictionary<string, Room>();
            _rooms.TryAdd("Entrance", new StartRoom(eventQueue, sessions, this));
            _rooms.TryAdd("EndRoom", new EndRoom(eventQueue, sessions, this));
            _rooms.TryAdd("Square", new Square(eventQueue, sessions, this));
            _rooms.TryAdd("Warehouse", new Warehouse(eventQueue, sessions, this));
        }

        public Room GetRoom(string id)
        {
            Room room = null;
            if (!_rooms.TryGetValue(id, out room))
            {
                throw new InvalidOperationException();
            }
            return room;
        }
    }
}
