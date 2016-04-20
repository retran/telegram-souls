using Autofac;
using System;
using System.Configuration;
using TelegramSouls.Server.Telegram;
using TelegramSouls.Server.World;

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
            builder.RegisterInstance(new EventQueue()).As<EventQueue>();
            builder.RegisterType<SessionContextFactory>().SingleInstance();
            builder.RegisterType<SessionStorage>().SingleInstance();
            builder.RegisterType<MessagePoller>().SingleInstance();
            builder.RegisterType<MessageHandler>().SingleInstance();
            builder.RegisterType<MessageSender>().SingleInstance();
            builder.RegisterType<EventHandler>().SingleInstance();
            builder.RegisterType<Area>().SingleInstance();

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<EventHandler>().Start();
                scope.Resolve<MessageHandler>().Start();
                scope.Resolve<MessagePoller>().Start();
                Console.ReadKey();
            }
        }
    }
}
