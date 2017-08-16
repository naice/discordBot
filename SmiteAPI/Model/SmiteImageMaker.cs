using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteAPI.Model
{
    public enum SmiteImageMakerImageType { LastMatch, Player, RankedStats }
    public class SmiteImageMaker
    {
        public SmiteImageMakerImageType ImageType { get; set; }
    }
    public class SmiteImageMaker<T> : SmiteImageMaker
    {
        public T Data { get; set; }
    }
}
