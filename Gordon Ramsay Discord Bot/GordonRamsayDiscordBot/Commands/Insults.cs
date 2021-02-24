using Discord;
using Discord.Commands;
using System.Text;
using System.Threading.Tasks;
using GordonRamsayDiscordBot;

namespace GordonRamsayBot.Commands
{
    [RequireContext(ContextType.Guild)]
    [Group("insults")]
    public class Insults : ModuleBase
    {
        [IsBotOwner]
        [Command("add")]
        public async Task AddNewInsult([Remainder] string insult)
        {
            // Add new insult to list!
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
            ArrayHandler.ReloadInsults();
            ArrayHandler.ReloadMasterList();
            await Context.Channel.SendMessageAsync($"New insult added: {insult}");
        }

        [Command("count")]
        public async Task Count()
        {
            var c = ArrayHandler.MasterList.Count;
            var ic = ArrayHandler.gInsults.Count;
            await Context.Channel.SendMessageAsync($"Total insults: {c}\nRemaining Insults: {ic}");
        }

        [IsBotOwner]
        [Command("delete")]
        public async Task DeleteInsult([Remainder] string insult)
        {
            var ip = false;
            var ml = false;
            for (int i = 0; i < ArrayHandler.gInsults.Count; i++)
            {
                if (!ip && ArrayHandler.gInsults[i].ToLower().Equals(insult.ToLower()))
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

            var sb = new StringBuilder();
            if (!ip || !ml)
                sb.Append("That insult wasn't found" + (!ip && !ml ? " in either list." : ml ? " in the in-progress list." : " in the master list."));
            else
                sb.Append($"New insult count:\nTotal: {ArrayHandler.MasterList.Count}\nIn-Progress: {ArrayHandler.gInsults.Count}");
            await Context.Channel.SendMessageAsync(sb.ToString());
        }

        [Command("find")]
        public async Task FindInsult([Remainder] string q)
        {
            var sb = new StringBuilder();
            var i = 0;
            for (int ii = 0; ii < ArrayHandler.MasterList.Count; ii++)
            {
                var s = ArrayHandler.MasterList[ii];
                if (s.ToLower().Contains(q.ToLower()))
                    sb.Append($"{++i}. {s}\n");
            }

            await ReplyAsync("", false, new EmbedBuilder()
                .WithTitle($"Insults containing {q}")
                .WithDescription(sb.ToString())
                .WithColor(Colours.Blue)
                .Build());
        }
    }
}