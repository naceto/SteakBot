﻿using System.Threading.Tasks;
using Discord.Commands;

namespace SteakBot.Core.Modules
{
    public class ChatModule : ModuleBase<SocketCommandContext>
    {
        [Command("hi")]
        [Summary("Bot says \"Hi\"")]
        public async Task Hi()
        {
            await ReplyAsync($"Hi, {Context.User.Mention} !");
        }

        [Command("say")]
        [Summary("`say <something>` - Bot says something")]
        public async Task Say([Remainder]string message)
        {
            await ReplyAsync(message);
        }
    }
}
