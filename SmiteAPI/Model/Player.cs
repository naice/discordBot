using System;
using System.Collections.Generic;
using System.Text;

namespace SmiteAPI.Model
{
    public class Player : ResponseBase
    {
        public string Avatar_URL { get; set; }
        public string Created_Datetime { get; set; }
        public int Id { get; set; }
        public string Last_Login_Datetime { get; set; }
        public int Leaves { get; set; }
        public int Level { get; set; }
        public int Losses { get; set; }
        public int MasteryLevel { get; set; }
        public string Name { get; set; }
        public string Personal_Status_Message { get; set; }
        public int Rank_Stat_Conquest { get; set; }
        public int Rank_Stat_Duel { get; set; }
        public int Rank_Stat_Joust { get; set; }
        public RankedConquest RankedConquest { get; set; }
        public RankedDuel RankedDuel { get; set; }
        public RankedJoust RankedJoust { get; set; }
        public string Region { get; set; }
        public int TeamId { get; set; }
        public string Team_Name { get; set; }
        public int Tier_Conquest { get; set; }
        public int Tier_Duel { get; set; }
        public int Tier_Joust { get; set; }
        public int Total_Achievements { get; set; }
        public int Total_Worshippers { get; set; }
        public int Wins { get; set; }
    }
}
