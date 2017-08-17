using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteImageMaker
{
    public class DesignTimeMock<Mock> where Mock : new()
    {
        public Mock Data { get; set; }

        public DesignTimeMock(string json)
        {
            Data = Newtonsoft.Json.JsonConvert.DeserializeObject<Mock>(json);
        }
    }
}
