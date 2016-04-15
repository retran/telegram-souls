using Newtonsoft.Json;

namespace TelegramSouls.Server.Telegram
{
    public class Location
    {
        [JsonProperty("id")]
        public double Longitude;

        [JsonProperty("first_name")]
        public double Latitude;
    }
}
