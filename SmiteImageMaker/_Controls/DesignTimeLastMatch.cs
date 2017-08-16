using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteImageMaker
{
    public class DesignTimeLastMatch : SmiteAPI.Model.MatchHistory
    {
        public DesignTimeLastMatch()
        {
            playerName = "Emmuss";
            Kills = 35;
            Deaths = 0;
            Assists = 26;
            Damage = 145000;
            Match = 358191335;
            Win_Status = "Win";
            Multi_kill_Max = 5;
            Queue = "Conquest";
            God = "Fenrir";
            Minutes = 45;
            Gold = 100000;
        }
    }
}
