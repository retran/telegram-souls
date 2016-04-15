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
            var c = new Client("<your key here>");
            var me = await c.GetMe();
            var updates = await c.GetUpdates(null);
            var last = updates.Result.Last();
            var offset = last.UpdateId + 1;
            while (true)
            {
                var upd = await c.GetUpdates(new GetUpdatesQuery()
                {
                    Offset = offset
                });

                foreach (var m in upd.Result)
                {
                    var message = await c.SendMessage(new SendMessageQuery()
                    {
                        ChatId = m.Message.Chat.Id,
                        Text = "TEST",
                        ReplyToMessageId = m.Message.MessageId
                    });
                    offset = m.UpdateId + 1;
                }
            }
        }
    }
}
