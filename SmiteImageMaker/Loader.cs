using Model=SmiteAPI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteImageMaker
{
    internal class Loader
    {
        public System.Windows.Controls.Control Control { get; set; }

        public Loader(string inputFile)
        {
            string fileContents = File.ReadAllText(inputFile);
            var typeGetter = Deserialize<Model.SmiteImageMaker>(fileContents);
            var imageType = typeGetter.ImageType;

            switch (imageType)
            {
                case Model.SmiteImageMakerImageType.LastMatch:
                    Control = new LastMatchControl() { DataContext = Deserialize<Model.SmiteImageMaker<Model.MatchHistory>>(fileContents).Data };
                    break;
                case Model.SmiteImageMakerImageType.Player:
                    break;
                case Model.SmiteImageMakerImageType.RankedStats:
                    break;
                default:
                    break;
            }
        }

        private static T Deserialize<T>(string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }
}
