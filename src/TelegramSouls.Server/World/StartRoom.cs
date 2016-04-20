using System.Collections.Concurrent;

namespace TelegramSouls.Server.World
{
    public class StartRoom : Room
    {
        public StartRoom(EventQueue eventQueue, SessionStorage sessions, Area area)
        {
            Id = "StartRoom";

            _sessions = sessions;
            _events = eventQueue;            
            _area = area;
            _directions = new ConcurrentDictionary<string, string>();
            _directions.TryAdd("N", "EndRoom");
        }

        public override string GetTitle(SessionContext context)
        {
            return "Южная комната";
        }

        public override string GetDescription(SessionContext context)
        {
            return "Проход ведет на север.";
        }
    }

    public class EndRoom : Room
    {
        public EndRoom(EventQueue eventQueue, SessionStorage sessions, Area area)
        {
            Id = "EndRoom";

            _sessions = sessions;
            _events = eventQueue;
            _area = area;
            _directions = new ConcurrentDictionary<string, string>();
            _directions.TryAdd("S", "StartRoom");
        }

        public override string GetTitle(SessionContext context)
        {
            return "Северная комната";
        }

        public override string GetDescription(SessionContext context)
        {
            return "Проход ведет на юг.";
        }
    }

}
