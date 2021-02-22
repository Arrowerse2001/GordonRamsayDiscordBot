using Discord;
using Discord.Commands;
using GordonRamsayBot.Handlers;
using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord.WebSocket;
using System.Collections.Generic;

namespace GordonRamsayBot.Commands
{
    [RequireContext(ContextType.Guild)]
    [Group("insults")]
    public class Insults : ModuleBase
    {
        [Command("add")]
        public async Task AddNewInsult([Remainder]string insult)
        {
            if (Context.User.Id != 470033984575897600)
            {
                await Context.Channel.SendMessageAsync("Only the bot developer can do that. Use ``gr!suggest``");
            } else
            {
                // Add new topic to list!
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"MasterList.txt", true))
                {
                    file.Write($"\n{insult}");
                }

                // Add to in progress
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"insultsProgress", true))
                {
                    file.Write($"\n{insult}");
                }

                // Reload
                GordonRamsayBot.ArrayHandler.ReloadInsults();
                GordonRamsayBot.ArrayHandler.ReloadMasterList();
                await Context.Channel.SendMessageAsync($"New insult added: {insult}");
            }
        }

        [Command("count")]
        public async Task Count()
        {
            int c = GordonRamsayBot.ArrayHandler.MasterList.Count;
            int ic = GordonRamsayBot.ArrayHandler.gInsults.Count;
            await Context.Channel.SendMessageAsync($"Total insults: {c}\nRemaining Insults: {ic}");
        }

        [Command("delete")]
        public async Task DeleteInsult([Remainder] string insult)
        {
            if (Context.User.Id != 470033984575897600)
            {
                await Context.Channel.SendMessageAsync($"Only the bot developer can do that.");
            }
            else
            {
                bool ip = false;
                bool ml = false;
                for (int i = 0; i < ArrayHandler.gInsults.Count; i++)
                {
                    if (!ip && GordonRamsayBot.ArrayHandler.gInsults[i].ToLower().Equals(insult.ToLower()))
                    {
                        ip = true;
                        ArrayHandler.gInsults.RemoveAt(i);
                        ArrayHandler.RewriteInsults();
                        await Context.Channel.SendMessageAsync($"Insult deleted from list!");
                        break;
                    }
                }

                for (int i = 0; i < ArrayHandler.MasterList.Count; i++)
                {
                    if (!ml && ArrayHandler.MasterList[i].ToLower().Equals(insult.ToLower()))
                    {
                        ml = true;
                        ArrayHandler.MasterList.RemoveAt(i);
                        ArrayHandler.RewriteMasterListInsults();
                        await Context.Channel.SendMessageAsync($"Insult deleted from master list");
                        break;
                    }
                }

                StringBuilder sb = new StringBuilder();
                if (!ip || !ml)
                    sb.Append("That insult wasn't found" + (!ip && !ml ? " in either list." : ml ? " in the in-progress list." : " in the master list."));
                else
                    sb.Append($"New insult count:\nTotal: {ArrayHandler.MasterList.Count}\nIn-Progress: {ArrayHandler.gInsults.Count}");
                await Context.Channel.SendMessageAsync(sb.ToString());
            }
        }

        [Command("find")]
        public async Task FindInsult([Remainder]string q)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            for (int ii = 0; ii < ArrayHandler.MasterList.Count; ii++)
            {
                string s = ArrayHandler.MasterList[ii];
                if (s.ToLower().Contains(q.ToLower()))
                    sb.Append($"{++i}. {s}\n");
            }
            // Embed
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"Insults containing {q}")
                .WithDescription(sb.ToString())
                .WithColor(Colours.Blue);
            await ReplyAsync("", false, builder.Build());
        }
    }
}
