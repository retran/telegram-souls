using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server.World
{
    public abstract class Room
    {
        protected ConcurrentDictionary<string, string> _directions;
        protected EventQueue _events;
        protected Area _area;
        protected SessionStorage _sessions;

        public string Id { get; protected set; }

        public virtual string GetTitle(SessionContext context)
        {
            return "Generic Room";
        }

        public virtual string GetDescription(SessionContext context)
        {
            return "Generic Room";
        }

        public virtual KeyboardButton[] GetContextActions(SessionContext session)
        {
            var buttons = new List<KeyboardButton>();

            buttons.Add(new KeyboardButton()
            {
                Text = "|Смотреть|"
            });

            return buttons.ToArray();
        }


        public virtual KeyboardButton[] GetNavigationalButtons(SessionContext session)
        {
            var buttons = new List<KeyboardButton>();

            if (_directions.ContainsKey("N"))
            {
                buttons.Add(new KeyboardButton()
                {
                    Text = "|Север|"
                });
            }

            if (_directions.ContainsKey("S"))
            {
                buttons.Add(new KeyboardButton()
                {
                    Text = "|Юг|"
                });
            }

            if (_directions.ContainsKey("W"))
            {
                buttons.Add(new KeyboardButton()
                {
                    Text = "|Запад|"
                });
            }

            if (_directions.ContainsKey("E"))
            {
                buttons.Add(new KeyboardButton()
                {
                    Text = "|Восток|"
                });
            }

            return buttons.ToArray();
        }

        public virtual void Go(SessionContext context, string direction)
        {
            if (_directions.ContainsKey(direction))
            {
                string to;
                if (_directions.TryGetValue(direction, out to))
                {
                    _events.Enqueue(new GoEvent(context, _events.Turn + 1, this, _area.GetRoom(to)));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                context.Reply("Вы не можете пойти в эту сторону.");
            }
        }

        public virtual void GoNorth(SessionContext context)
        {
            Go(context, "N");
        }

        public virtual void GoSouth(SessionContext context)
        {
            Go(context, "S");
        }

        public virtual void GoWest(SessionContext context)
        {
            Go(context, "W");
        }

        public virtual void GoEast(SessionContext context)
        {
            Go(context, "E");
        }

        public virtual void Look(SessionContext context)
        {
            string description = string.Format("{0}\n{1}", GetTitle(context), GetDescription(context));

            var sessions = _sessions.GetSessions().Where(s => string.Equals(s.RoomId, this.Id));
            foreach (var s in sessions)
            {
                description += string.Format("\n{0} стоит здесь.", s.Username);
            }

            context.Send(description);
        }
    }
}