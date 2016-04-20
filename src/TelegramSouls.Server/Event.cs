namespace TelegramSouls.Server
{
    public abstract class Event
    {
        public SessionContext Context { get; set; }
        public long Turn { get; set; }

        public abstract void Handle();
    }
}