using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server.World
{
    public class StartRoom : Room
    {
        public StartRoom(EventQueue eventQueue, SessionStorage sessions, Area area)
        {
            Id = "Entrance";

            _sessions = sessions;
            _events = eventQueue;            
            _area = area;
            _directions = new ConcurrentDictionary<string, string>();
            _directions.TryAdd("N", "Square");
        }

        public override string GetTitle(SessionContext context)
        {
            return "Вход";
        }

        public override string GetDescription(SessionContext context)
        {
            return "Вы стоите перед большими воротами с надписью \"Добро пожаловать!\".\nНа воротах висит табличка.\nВы можете пройти сквозь ворота на север.";
        }

        public override KeyboardButton[] GetContextActions(SessionContext session)
        {
            var buttons = new List<KeyboardButton>();

            buttons.Add(new KeyboardButton()
            {
                Text = "|Смотреть|"
            });

            buttons.Add(new KeyboardButton()
            {
                Text = "|Осмотреть табличку|"
            });

            return buttons.ToArray();
        }

        public override bool ProcessContextAction(SessionContext context, string text)
        {
            if (string.Equals(text, "|Осмотреть табличку|", StringComparison.OrdinalIgnoreCase))
            {
                var description = "На табличке написано: \"Чтобы сказать что-то другим игрокам - напиши это как в обычном чате.\"";
                context.Send(description);

                return true;
            }

            return base.ProcessContextAction(context, text);
        }
    }

    public class Warehouse : Room
    {
        public Warehouse(EventQueue eventQueue, SessionStorage sessions, Area area)
        {
            Id = "Warehouse";

            _sessions = sessions;
            _events = eventQueue;
            _area = area;
            _directions = new ConcurrentDictionary<string, string>();
            _directions.TryAdd("W", "Square");
        }

        public override string GetTitle(SessionContext context)
        {
            return "Склад";
        }

        public override string GetDescription(SessionContext context)
        {
            return "Вы стоите в большой пыльной комнате заваленной заколоченными ящиками.";
        }

        public override KeyboardButton[] GetContextActions(SessionContext session)
        {
            var buttons = new List<KeyboardButton>();

            buttons.Add(new KeyboardButton()
            {
                Text = "|Смотреть|"
            });

            buttons.Add(new KeyboardButton()
            {
                Text = "|Осмотреть ящики|"
            });

            return buttons.ToArray();
        }

        public override bool ProcessContextAction(SessionContext context, string text)
        {
            if (string.Equals(text, "|Осмотреть ящики|", StringComparison.OrdinalIgnoreCase))
            {
                var description = "Вам удается рассмотреть, что на ближайших ящиках висят таблички: \"Мобы. Не открывать\", \"Эквипмент для персонажей\", \"Триггеры\", \"Тулсет\".";
                context.Send(description);

                return true;
            }

            return base.ProcessContextAction(context, text);
        }
    }

    public class Square : Room
    {
        public Square(EventQueue eventQueue, SessionStorage sessions, Area area)
        {
            Id = "Square";

            _sessions = sessions;
            _events = eventQueue;
            _area = area;
            _directions = new ConcurrentDictionary<string, string>();
            _directions.TryAdd("S", "Entrance");
            _directions.TryAdd("E", "Warehouse");
            _directions.TryAdd("N", "EndRoom");

        }

        public override string GetTitle(SessionContext context)
        {
            return "Площадь";
        }

        public override string GetDescription(SessionContext context)
        {
            return "Вы стоите посреди большого пустыря заваленного досками, оберточной бумагой и табличками \"Under construction\".\nНа юге виднеются ворота, на востоке расположено большое здание. \nНа севере воздух плавно темнеет скрывая обстановку.";
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
            _directions.TryAdd("S", "Square");
        }

        public override string GetTitle(SessionContext context)
        {
            return "Край мира";
        }

        public override string GetDescription(SessionContext context)
        {
            return "Здесь заканчивается мир. Вы видите как земля под ногами плавно превращается в россыпь нулей и единиц и исчезает вдали.\nВы можете вернуться на юг.";
        }
    }

}
