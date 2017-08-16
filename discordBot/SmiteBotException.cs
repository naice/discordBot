using System;
using System.Collections.Generic;
using System.Text;

namespace discordBot
{
    public class SmiteBotException : Exception
    {
        public SmiteBotException(string msg) : base(msg)
        {

        }
    }
}
