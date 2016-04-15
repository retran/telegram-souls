using Newtonsoft.Json;

namespace TelegramSouls.Server.Telegram
{
    public class GetUpdatesQuery
    {
        [JsonProperty("offset")]
        public long? Offset;

        [JsonProperty("limit")]
        public long? Limit;

        [JsonProperty("timeout")]
        public long? Timeout;
    }
}