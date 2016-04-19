namespace TelegramSouls.Server
{
    public class SessionContext
    {
        public long Id { get; private set; }
        public string Username { get; private set; }
        public string Room { get; set; }
        public bool InGame { get; set; }

        public SessionContext(long id, string username)
        {
            Id = id;
            Username = username;
        }

        public void Load()
        {
            Room = "Default";
            InGame = false;
        }

        public void Persist()
        {
        }
    }
}
