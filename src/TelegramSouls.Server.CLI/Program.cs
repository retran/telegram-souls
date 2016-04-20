using Autofac;
using System;
using System.Configuration;
using TelegramSouls.Server.Telegram;

namespace TelegramSouls.Server.CLI
{
    class Program
    {
        static void Main()
        {
            var token = ConfigurationManager.AppSettings["telegramApiToken"];
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new TelegramClient(token)).As<TelegramClient>();
            builder.RegisterInstance(new MessageQueue()).As<MessageQueue>();
            builder.RegisterType<SessionStorage>().SingleInstance();
            builder.RegisterType<MessagePoller>().SingleInstance();
            builder.RegisterType<MessageHandler>().SingleInstance();
            builder.RegisterType<MessageSender>().SingleInstance();

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<MessagePoller>().Start();
                scope.Resolve<MessageHandler>().Start();
                Console.ReadKey();
            }
        }
    }
}
