using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace discordBot
{
    public class Config
    {
        public string BotToken { get; set; } /// DiscordBotToken
        public string SmiteDevId { get; set; }
        public string SmiteAuthKey { get; set; }
        public string SmiteEndpoint { get; set; }
        public string SmiteDataCache { get; set; }
        public string SmiteImageMakerExcutable { get; set; }
        public string SmiteImageMakerResultFolderPath { get; set; }

        public static Config Load(string fromPath)
        {
            if (File.Exists(fromPath))
            {
                try
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(fromPath));
                }
                catch (IOException)
                {

                }
            }

            return new Config();
        }
    }
}
