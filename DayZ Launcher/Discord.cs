using System;
using DiscordRPC;
using DiscordRPC.Logging;

namespace DayZ_Launcher
{
    internal class Discord
    {
        public static DiscordRpcClient client;

        public static void Initialize()
        {
            // Evita inicializar duas vezes
            if (client != null && client.IsInitialized)
                return;

            client = new DiscordRpcClient("975134151814570034")
            {
                Logger = new ConsoleLogger
                {
                    Level = LogLevel.Warning
                }
            };

            client.OnReady += (sender, e) =>
            {
                Console.WriteLine($"Received Ready from user {e.User.Username}");
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine($"Received Update! {e.Presence}");
            };

            client.Initialize();

            changeDiscordRPC("Running Launcher", "", "DayZ Launcher", "logo");
        }

        public static void changeDiscordRPC(
            string status,
            string details,
            string imageText,
            string imageKey)
        {
            if (client == null || !client.IsInitialized)
                return;

            client.SetPresence(new RichPresence
            {
                State = status,
                Details = details,
                Assets = new Assets
                {
                    LargeImageKey = imageKey,
                    LargeImageText = imageText
                }
            });
        }
    }
}
