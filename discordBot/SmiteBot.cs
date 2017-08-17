using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using System.IO;
using System.Diagnostics;

namespace discordBot
{
    public class SmiteBot : IDiscordMessageBot
    {

        private SmiteAPI.Smite Smite;
        private readonly string _smiteImageMakerPath;
        private readonly string _smiteImageMakerResultFolderPath;
        private readonly string _smiteDataCacheFile;
        private SmiteAPI.Model.God[] _godsCache;
        private SmiteAPI.Model.Item[] _itemsCache;

        public SmiteBot(Config config)
        {
            _smiteImageMakerResultFolderPath = config.SmiteImageMakerResultFolderPath;
            _smiteImageMakerPath = config.SmiteImageMakerExcutable;
            _smiteDataCacheFile = config.SmiteDataCache;

            Smite = new SmiteAPI.Smite(new SmiteAPI.Config(config.SmiteDevId, config.SmiteAuthKey) { SmiteEndpoint = config.SmiteEndpoint });
        }

        public class SmiteDataCache
        {
            public SmiteAPI.Model.God[] Gods { get; set; }
            public SmiteAPI.Model.Item[] Items { get; set; }
        }
        private async Task RefreshCacheIfNecessary()
        {
            var godCacheLastRefresh = DateTime.MinValue;
            if (File.Exists(_smiteDataCacheFile))
                godCacheLastRefresh = File.GetLastWriteTimeUtc(_smiteDataCacheFile);
            if (DateTime.UtcNow - godCacheLastRefresh < TimeSpan.FromHours(24))
            {
                if (_godsCache == null || _itemsCache == null)
                {
                    var cache = Newtonsoft.Json.JsonConvert.DeserializeObject<SmiteDataCache>(
                        File.ReadAllText(_smiteDataCacheFile));

                    _godsCache = cache.Gods;
                    _itemsCache = cache.Items;
                }
                return;
            }

            var gods = await Smite.GetGods(1);
            var items = await Smite.GetItems(1);
            if (gods != null && gods.Length > 0 &&
                items != null && items.Length > 0)
            {
                _godsCache = gods;
                _itemsCache = items;
                File.WriteAllText(_smiteDataCacheFile, Newtonsoft.Json.JsonConvert.SerializeObject(
                    new SmiteDataCache() { Gods = gods, Items = items }));
            }            
        }

        public async Task ProcessMessage(SocketMessage msg)
        {
            var cmd = new DiscordBotCommandParser(".smite", msg.Content);

            if (cmd.Success)
            {
                await RefreshCacheIfNecessary();

                var rankedMatch = cmd.Match("ranked");
                if (rankedMatch.Success)
                {
                    var lookupName = rankedMatch.Matches["ranked"];
                    await SendPlayerRanked(lookupName, msg.Channel);
                    return;
                }

                var matchMatch = cmd.Match("match");
                if (matchMatch.Success)
                {
                    var lookupName = matchMatch.Matches["match"];
                    await SendPlayerLastMatch(lookupName, msg.Channel);
                    return;
                }

                if (cmd.Result.Length > 0)
                {
                    var lookupName = cmd.Result[0];
                    if (lookupName.Length > 3)
                    {
                        await SendPlayer(lookupName, msg.Channel);
                        return;
                    }
                }
            }
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
            catch (Exception ex)
            {
                throw ex;
            }

#if RELEASE
            File.Delete(uniqueDataPath);
#endif
            return null;
        }
        private async Task SendImageAndDelete(string lookupName, ISocketMessageChannel channel, SmiteAPI.Model.SmiteImageMaker<object> imageMakerData)
        {
            var imageFile = RunSmiteImageMaker(imageMakerData);
            if (imageFile == null)
            {
                throw new SmiteBotException($"Unable to generate Image for {lookupName}");
            }
            await channel.SendFileAsync(imageFile);

            File.Delete(imageFile);
        }

        private async Task SendPlayerLastMatch(string lookupName, ISocketMessageChannel channel)
        {
            var lastMatches = await Smite.GetMatchHistory(lookupName);
            if (lastMatches == null || lastMatches.Length <= 0)
            {
                throw new SmiteBotException($"No Matchdata found for {lookupName}!");
            }

            var lastMatch = lastMatches[0];
            if (lastMatch == null || !string.IsNullOrEmpty(lastMatch.ret_msg))
            {
#if DEBUG
                throw new SmiteBotException($"No Matchdata found for {lookupName}! DEBUG-MSG:{lastMatch.ret_msg}");
#else
                throw new SmiteBotException($"No Matchdata found for {lookupName}!");
#endif
            }

            var imageMakerData = new SmiteAPI.Model.SmiteImageMaker<object>()
            {
                GodCache = _godsCache,
                ItemCache = _itemsCache,
                ImageType = SmiteAPI.Model.SmiteImageMakerImageType.LastMatch,
                Data = lastMatch
            };

            await SendImageAndDelete(lookupName, channel, imageMakerData);
        }
        private async Task SendPlayerRanked(string lookupName, ISocketMessageChannel channel)
        {
            var player = await Smite.GetPlayer(lookupName);
            if (!IsPlayerValid(lookupName, player))
            {
                throw new SmiteBotException($"The player {lookupName} is unknown!");
            }

            var imageMakerData = new SmiteAPI.Model.SmiteImageMaker<object>()
            {
                GodCache = _godsCache,
                ItemCache = _itemsCache,
                ImageType = SmiteAPI.Model.SmiteImageMakerImageType.RankedStats,
                Data = player
            };

            await SendImageAndDelete(lookupName, channel, imageMakerData);
        }
        private async Task SendPlayer(string lookupName, ISocketMessageChannel channel)
        {
            var player = await Smite.GetPlayer(lookupName);
            if (!IsPlayerValid(lookupName, player))
            {
                throw new SmiteBotException($"The player {lookupName} is unknown!");
            }

            var imageMakerData = new SmiteAPI.Model.SmiteImageMaker<object>()
            {
                GodCache = _godsCache,
                ItemCache = _itemsCache,
                ImageType = SmiteAPI.Model.SmiteImageMakerImageType.Player,
                Data = player
            };

            await SendImageAndDelete(lookupName, channel, imageMakerData);
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
