using System;

namespace SmiteAPI
{
    public class Config
    {
        public string DevId { get; set; }
        public string DevAuthKey { get; set; }
        public string SmiteEndpoint { get; set; } = "http://api.smitegame.com/smiteapi.svc";

        public Config(string devId, string devAuthKey)
        {
            DevId = devId;
            DevAuthKey = devAuthKey;
        }
    }
}
