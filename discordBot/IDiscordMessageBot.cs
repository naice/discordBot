﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace discordTests
{
    interface IDiscordMessageBot
    {
        Task ProcessMessage(SocketMessage msg);
    }
}
