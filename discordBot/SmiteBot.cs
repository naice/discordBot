using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using System.IO;
using System.Diagnostics;

namespace discordTests
{
    public class SmiteBot : IDiscordMessageBot
    {
        public class SmiteBotException : Exception
        {
            public SmiteBotException(string msg) : base(msg)
            {

            }
        }

        private SmiteAPI.Smite Smite;
        private readonly string _smiteImageMakerPath;
        private readonly string _smiteImageMakerResultFolderPath;

        public SmiteBot(Config config)
        {
            _smiteImageMakerResultFolderPath = config.SmiteImageMakerResultFolderPath;
            _smiteImageMakerPath = config.SmiteImageMakerExcutable;

            Smite = new SmiteAPI.Smite(new SmiteAPI.Config(config.SmiteDevId, config.SmiteAuthKey) { SmiteEndpoint = config.SmiteEndpoint });
        }


        public async Task ProcessMessage(SocketMessage msg)
        {
            var cmd = new DiscordBotCommandParser(".smite", msg.Content);

            if (cmd.Success)
            {
                var rankedMatch = cmd.Match("ranked");
                if (rankedMatch.Success)
                {
                    var lookupName = rankedMatch.Matches["ranked"];
                    var text = await PrintPlayerRanked(lookupName);

                    if (!string.IsNullOrEmpty(text))
                    {
                        await SendMessageAsync(msg.Channel, text);
                    }
                    return;
                }

                var matchMatch = cmd.Match("match");
                if (matchMatch.Success)
                {
                    var lookupName = matchMatch.Matches["match"];


                    await SendPlayerLastMatchEmbed(lookupName, msg.Channel);
                    return;
                }

                if (cmd.Result.Length > 0)
                {
                    var lookupName = cmd.Result[0];
                    if (lookupName.Length > 3)
                    {
                        var text = await PrintPlayer(lookupName);

                        if (!string.IsNullOrEmpty(text))
                        {
                            await SendMessageAsync(msg.Channel, text);
                        }
                        return;
                    }
                }
            }
        }

        private Task SendMessageAsync(ISocketMessageChannel channel, string text)
        {
            return channel.SendMessageAsync($"```\n{text}```");
        }
        private async Task SendImageAndDeleteAsync(ISocketMessageChannel channel, string file)
        {
            await channel.SendFileAsync(file);

            File.Delete(file);
        }
        private bool IsPlayerValid(string lookupName, SmiteAPI.Model.Player player)
        {
            if (player != null && player.Name.ToLower().Contains(lookupName.ToLower()))
                return true;

            return false;
        }

        private string RunSmiteImageMaker(SmiteAPI.Model.SmiteImageMaker<object> data)
        {
            var guid = Guid.NewGuid();
            string uniqueImagePath = Path.Combine(_smiteImageMakerResultFolderPath, $"{guid}.png");
            string uniqueDataPath = Path.Combine(_smiteImageMakerResultFolderPath, $"{guid}.json");

            try
            {
                File.WriteAllText(uniqueDataPath, Newtonsoft.Json.JsonConvert.SerializeObject(data));
                Process process = Process.Start(_smiteImageMakerPath, $"\"{uniqueDataPath}\" \"{uniqueImagePath}\" ");
                process.WaitForExit();

                if (File.Exists(uniqueImagePath))
                    return uniqueImagePath;
            }
            catch (Exception)
            {

            }

#if RELEASE
            File.Delete(uniqueDataPath);
#endif
            return null;
        }

        private async Task SendPlayerLastMatchEmbed(string lookupName, ISocketMessageChannel channel)
        {
            var lastMatches = await Smite.GetMatchHistory(lookupName);
            if (lastMatches == null || lastMatches.Length <= 0)
            {
                throw new SmiteBotException($"No Matchdata found for {lookupName}!");
            }

            var lastMatch = lastMatches[0];
            if (lastMatch == null || !string.IsNullOrEmpty(lastMatch.ret_msg))
            {
                throw new SmiteBotException($"No Matchdata found for {lookupName}!");
            }

            var imageMakerData = new SmiteAPI.Model.SmiteImageMaker<object>();
            imageMakerData.ImageType = SmiteAPI.Model.SmiteImageMakerImageType.LastMatch;
            imageMakerData.Data = lastMatch;

            var imageFile = RunSmiteImageMaker(imageMakerData);
            if (imageFile != null)
            {
                await channel.SendFileAsync(imageFile);
            }
        }
        
        private async Task<string> PrintPlayerRanked(string lookupName)
        {
            var player = await Smite.GetPlayer(lookupName);
            if (!IsPlayerValid(lookupName, player))
            {
                return $"The player {lookupName} is unknown!";
            }

            StringBuilder text = new StringBuilder();
            text.AppendLine($"Ranked stats of {player.Name}");
            if (player.RankedConquest != null)
                text.AppendLine($"Conquest: {player.RankedConquest.Wins} Wins, {player.RankedConquest.Losses} Losses");
            if (player.RankedJoust != null)
                text.AppendLine($"Joust: {player.RankedJoust.Wins} Wins, {player.RankedJoust.Losses} Losses");
            if (player.RankedDuel != null)
                text.AppendLine($"Duel: {player.RankedDuel.Wins} Wins, {player.RankedDuel.Losses} Losses");
            if (player.RankedConquest == null && player.RankedJoust == null && player.RankedDuel == null)
                text.AppendLine("No ranked statistics found. :/");

            return text.ToString();
        }

        private async Task<string> PrintPlayer(string lookupName)
        {
            var player = await Smite.GetPlayer(lookupName);
            if (!IsPlayerValid(lookupName, player))
            {
                return $"The player {lookupName} is unknown!";
            }

            StringBuilder text = new StringBuilder();
            text.Append($"This is {lookupName}"); 
            if (!string.IsNullOrEmpty(player.Personal_Status_Message))
                text.Append($"   |   {player.Personal_Status_Message}");
            text.AppendLine();
            text.AppendLine($"LVL {player.Level}, {player.Wins} Wins, {player.Losses} Losses, {player.Team_Name??"no team"}");
            
            return text.ToString();
        }
    }
}
