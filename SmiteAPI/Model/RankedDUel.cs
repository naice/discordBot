using System;
using System.Collections.Generic;
using System.Text;

namespace SmiteAPI.Model
{
    public class RankedDuel
    {
        public int Leaves { get; set; }
        public int Losses { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public int PrevRank { get; set; }
        public int Rank { get; set; }
        public object Rank_Stat_Conquest { get; set; }
        public object Rank_Stat_Duel { get; set; }
        public object Rank_Stat_Joust { get; set; }
        public int Season { get; set; }
        public int Tier { get; set; }
        public int Trend { get; set; }
        public int Wins { get; set; }
        public object player_id { get; set; }
        public object ret_msg { get; set; }
    }
}
