using TelegramSouls.Server.World;

namespace TelegramSouls.Server
{
    public class GoEvent : Event
    {
        public GoEvent(SessionContext context, long turn, Room from, Room to)
        {
            Context = context;
            Turn = turn;
            From = from;
            To = to;
        }

        public Room From { get; private set; }
        public Room To { get; private set; }

        private void SendLeave()
        {
            Context.SendToRoom(string.Format("{0} ушел отсюда.", Context.Username));
        }

        private void SendCome()
        {
            Context.Send("Вы перешли в другую локацию.");
            To.Look(Context);
        }

        private void SendComeToRoom()
        {
            Context.SendToRoom(string.Format("{0} пришел сюда.", Context.Username));
        }

        public override void Handle()
        {
            SendLeave();
            Context.RoomId = To.Id;
            SendCome();
            SendComeToRoom();
        }
    }
}
