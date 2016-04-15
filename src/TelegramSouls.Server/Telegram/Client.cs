using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSouls.Server.Telegram
{
    public class Client
    {
        private readonly string _baseUri = "https://api.telegram.org/bot{0}/";

        private string _token;

        public Client(string token)
        {
            _token = token;
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(string.Format(_baseUri, _token));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
            return client;
        }

        public Task<TelegramResponse<User>> GetMe()
        {
            using (var c = GetHttpClient())
            {
                return c.PostAsJsonAsync("getMe", string.Empty).ContinueWith(task =>
                {
                    return task.Result.Content.ReadAsAsync<TelegramResponse<User>>();
                }).Result;
            }
        }

        public Task<TelegramResponse<Update[]>> GetUpdates(GetUpdatesQuery query)
        {
            using (var c = GetHttpClient())
            {
                return c.PostAsJsonAsync("getUpdates", query).ContinueWith(task =>
                {
                    return task.Result.Content.ReadAsAsync<TelegramResponse<Update[]>>();
                }).Result;
            }
        }

        public Task<TelegramResponse<Message>> SendMessage(SendMessageQuery query)
        {
            using (var c = GetHttpClient())
            {
                return c.PostAsJsonAsync("sendMessage", query).ContinueWith(task =>
                {
                    return task.Result.Content.ReadAsAsync<TelegramResponse<Message>>();
                }).Result;
            }
        }

    }
}
