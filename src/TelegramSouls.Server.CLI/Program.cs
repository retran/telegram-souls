using System;
using System.Linq;
using System.Threading.Tasks;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server.CLI
{
    class Program
    {
        static void Main()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            var c = new Client("188718743:AAEi9xE4Y8l-0q1KlqtsOYUEKViW6pw0y2A");
            var queue = new MessageQueue();

            var poller = new MessagePoller(c, queue);
            var handler = new MessageHandler(c, queue);

            poller.Start();
            handler.Start();

            Console.ReadKey();

            handler.Stop();
            poller.Stop();
        }
    }
}
