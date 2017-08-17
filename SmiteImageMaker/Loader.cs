using Model=SmiteAPI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Controls;

namespace SmiteImageMaker
{
    internal class Loader
    {
        public const string UPDATECACHEFILE = "update.cache";
        public Control Control { get; set; }

        private readonly string _cachePath;

        public Loader(string inputFile, string cachePath)
        {
            _cachePath = cachePath;
            string fileContents = File.ReadAllText(inputFile);
            var typeGetter = Deserialize<Model.SmiteImageMaker>(fileContents);
            var imageType = typeGetter.ImageType;

            switch (imageType)
            {
                case Model.SmiteImageMakerImageType.LastMatch:
                    var lastMatchModel = Deserialize<Model.SmiteImageMaker<Model.MatchHistory>>(fileContents);
                    UpdateBasicCache(lastMatchModel);
                    Control = new LastMatchControl() { DataContext = lastMatchModel };
                    break;
                case Model.SmiteImageMakerImageType.Player:
                    var playerModel = Deserialize<Model.SmiteImageMaker<Model.Player>>(fileContents);
                    UpdatePlayerCache(playerModel.Data);
                    Control = new PlayerControl() { DataContext = playerModel };
                    break;
                case Model.SmiteImageMakerImageType.RankedStats:
                    break;
                default:
                    break;
            }
        }

        private bool CheckCacheUpdateNecessary(Model.SmiteImageMaker data)
        {
            return !File.Exists(Path.Combine(_cachePath, UPDATECACHEFILE));
        }
        private void CreateCacheUpdateFile()
        {
            File.WriteAllText(Path.Combine(_cachePath, UPDATECACHEFILE), "IF REMOVED CACHE WILL REBUILT");
        }

        private void UpdateBasicCache(Model.SmiteImageMaker data)
        {
            if (!CheckCacheUpdateNecessary(data))
                return;

            foreach (var god in data.GodCache)
            {
                CacheFile(god.godCard_URL_Cache, god.godCard_URL);
                CacheFile(god.godIcon_URL_Cache, god.godIcon_URL);
            }
            foreach (var item in data.ItemCache)
            {
                CacheFile(item.itemIcon_URL_Cache, item.itemIcon_URL);
            }

            CreateCacheUpdateFile();
        }

        private void UpdatePlayerCache(Model.Player player)
        {
            if (!File.Exists(player.Avatar_URL_Cache))
            {
                CacheFile(player.Avatar_URL_Cache, player.Avatar_URL);
            }
        }

        private static void CacheFile(string path, string url)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(url, path);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private static T Deserialize<T>(string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }
}
