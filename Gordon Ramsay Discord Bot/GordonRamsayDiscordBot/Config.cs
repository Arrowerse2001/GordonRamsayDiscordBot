using System.IO;
using Newtonsoft.Json;
using GordonRamsayBot.Handlers;

namespace GordonRamsayBot
{
    static class Config
    {
        public static readonly BotConfig bot;

        static Config()
        {
            MinigameHandler.InitialTriviaSetup();

            if (!Directory.Exists("Resources"))
                Directory.CreateDirectory("Resources");

            // config.json
            if (!File.Exists("Resources/config.json"))
                File.WriteAllText("Resources/config.json", JsonConvert.SerializeObject(bot, Formatting.Indented));
            else
            {
                bot = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("Resources/config.json"));
            }
        }
    }

    public struct BotConfig
    {
        public string DiscordBotToken;
    }
}