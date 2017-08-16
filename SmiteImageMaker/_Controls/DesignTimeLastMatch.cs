using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteImageMaker
{
    public class DesignTimeLastMatch : SmiteAPI.Model.MatchHistory
    {
        const string json = @"{""ActiveId1"":7672,""ActiveId2"":14112,""Active_1"":""Aegis Amulet"",""Active_2"":""Bracer of Undoing"",""Active_3"":null,""Assists"":21,""Ban1"":"""",""Ban10"":"""",""Ban10Id"":0,""Ban1Id"":0,""Ban2"":"""",""Ban2Id"":0,""Ban3"":"""",""Ban3Id"":0,""Ban4"":"""",""Ban4Id"":0,""Ban5"":"""",""Ban5Id"":0,""Ban6"":"""",""Ban6Id"":0,""Ban7"":"""",""Ban7Id"":0,""Ban8"":"""",""Ban8Id"":0,""Ban9"":"""",""Ban9Id"":0,""Creeps"":75,""Damage"":15899,""Damage_Bot"":41625,""Damage_Done_In_Hand"":2530,""Damage_Mitigated"":14042,""Damage_Structure"":1842,""Damage_Taken"":26068,""Damage_Taken_Magical"":8757,""Damage_Taken_Physical"":17311,""Deaths"":4,""Distance_Traveled"":595568,""First_Ban_Side"":"""",""God"":""Fenrir"",""GodId"":1843,""Gold"":14075,""Healing"":0,""Healing_Bot"":0,""Healing_Player_Self"":5522,""ItemId1"":8268,""ItemId2"":8987,""ItemId3"":9626,""ItemId4"":7904,""ItemId5"":7641,""ItemId6"":8549,""Item_1"":""Death's Toll"",""Item_2"":""Bumba's Mask"",""Item_3"":""Warrior Tabi"",""Item_4"":""Jotunn's Wrath"",""Item_5"":""Breastplate of Valor"",""Item_6"":""Shifter's Shield"",""Killing_Spree"":3,""Kills"":5,""Level"":18,""Map_Game"":""Season 4 Conquest"",""Match"":358191335,""Match_Time"":""8\/15\/2017 6:34:28 PM"",""Minutes"":31,""Multi_kill_Max"":1,""Objective_Assists"":10,""Queue"":""Conquest"",""Region"":""Europe"",""Skin"":"""",""SkinId"":0,""Surrendered"":0,""TaskForce"":2,""Team1Score"":0,""Team2Score"":0,""Time_In_Match_Seconds"":1869,""Wards_Placed"":4,""Win_Status"":""Win"",""Winning_TaskForce"":2,""playerName"":""Emmuss"",""ret_msg"":null}";
        public DesignTimeLastMatch()
        {
            DesignTimeHelper.CreateMe<SmiteAPI.Model.MatchHistory>(this, json);

            Kills = 35;
            Deaths = 0;
            Assists = 26;
            Damage = 145000;
            Multi_kill_Max = 3;
            Minutes = 45;
            Gold = 100000;
        }
    }
}
