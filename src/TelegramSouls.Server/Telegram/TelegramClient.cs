using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TelegramSouls.Server.Telegram
{
    public class TelegramClient
    {
        private readonly string _baseUri = "https://api.telegram.org/bot{0}/";

        private string _token;

        public TelegramClient(string token)
        {
            _token = token;
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(string.Format(_baseUri, _token));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
            return client;
        }

        public Task<Response<User>> GetMe()
        {
            using (var c = CreateHttpClient())
            {
                return c.PostAsJsonAsync("getMe", string.Empty).ContinueWith(task =>
                {
                    return task.Result.Content.ReadAsAsync<Response<User>>();
                }).Result;
            }
        }

        public Task<Response<Update[]>> GetUpdates(GetUpdatesQuery query)
        {
            using (var c = CreateHttpClient())
            {
                return c.PostAsJsonAsync("getUpdates", query).ContinueWith(task =>
                {
                    return task.Result.Content.ReadAsAsync<Response<Update[]>>();
                }).Result;
            }
        }

        public Task<Response<Message>> SendMessage(SendMessageQuery query)
        {
            using (var c = CreateHttpClient())
            {
                return c.PostAsJsonAsync("sendMessage", query).ContinueWith(task =>
                {
                    return task.Result.Content.ReadAsAsync<Response<Message>>();
                }).Result;
            }
        }
    }
}
