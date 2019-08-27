﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using HtmlAgilityPack;

namespace SteakBot.Core.Modules
{
    class DotaModule : ModuleBase<SocketCommandContext>
    {
        [Command("prizepool")]
        public async Task List()
        {
            var uri = "http://www.dota2.com/international/battlepass/";
            var webParser = new HtmlWeb();
            var document = webParser.Load(uri);

            var prize = document.DocumentNode.SelectSingleNode("/html/div/div[3]/div[4]/div[3]/div[2]/div[2]")?.InnerText ?? "Unknown";

            await ReplyAsync($"The current prize pool for The International {DateTime.Now.Year} is: **{prize}**\r\nMore info at {uri}");
        }
    }
}
