using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteImageMaker
{
    public class DesignTimePlayer : SmiteAPI.Model.Player
    {
        private const string json = @"{""Avatar_URL"":""http:\/\/cds.q6u4m8x5.hwcdn.net\/web\/smite-app\/\/wp-content\/uploads\/2015\/06\/CountryFlag_Germany.png"",""Created_Datetime"":""4\/24\/2017 12:59:51 AM"",""Id"":12164228,""Last_Login_Datetime"":""8\/15\/2017 11:20:29 AM"",""Leaves"":12,""Level"":34,""Losses"":153,""MasteryLevel"":20,""Name"":""[GeOne] Emmuss"",""Personal_Status_Message"":""hallo an alle!"",""Rank_Stat_Conquest"":0,""Rank_Stat_Duel"":0,""Rank_Stat_Joust"":0,""RankedConquest"":{""Leaves"":1,""Losses"":7,""Name"":""Conquest"",""Points"":0,""PrevRank"":0,""Rank"":1000,""Rank_Stat_Conquest"":null,""Rank_Stat_Duel"":null,""Rank_Stat_Joust"":null,""Season"":4,""Tier"":0,""Trend"":0,""Wins"":1,""player_id"":null,""ret_msg"":null},""RankedDuel"":{""Leaves"":0,""Losses"":0,""Name"":""Duel"",""Points"":0,""PrevRank"":0,""Rank"":0,""Rank_Stat_Conquest"":null,""Rank_Stat_Duel"":null,""Rank_Stat_Joust"":null,""Season"":0,""Tier"":0,""Trend"":0,""Wins"":0,""player_id"":null,""ret_msg"":null},""RankedJoust"":{""Leaves"":0,""Losses"":0,""Name"":""Joust"",""Points"":0,""PrevRank"":0,""Rank"":0,""Rank_Stat_Conquest"":null,""Rank_Stat_Duel"":null,""Rank_Stat_Joust"":null,""Season"":0,""Tier"":0,""Trend"":0,""Wins"":0,""player_id"":null,""ret_msg"":null},""Region"":""Europe"",""TeamId"":556526,""Team_Name"":""GermanOnes"",""Tier_Conquest"":0,""Tier_Duel"":0,""Tier_Joust"":0,""Total_Achievements"":55,""Total_Worshippers"":2599,""Wins"":127,""ret_msg"":null}";
        public DesignTimePlayer()
        {
            DesignTimeHelper.CreateMe(this, json);
        }
    }
}
