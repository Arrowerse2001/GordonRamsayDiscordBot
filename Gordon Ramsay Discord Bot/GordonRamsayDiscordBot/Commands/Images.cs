using Discord;
using Discord.Commands;
using GordonRamsayDiscordBot.Handlers;
using System.Text;
using System.Threading.Tasks;

namespace GordonRamsayDiscordBot.Commands
{
    [Group("images")]
    [IsBotOwner]
    [RequireContext(ContextType.Guild)]
    public class Images : ModuleBase
    {
        [Command("add")]
        public async Task AddNewImage([Remainder] string imageLink)
        {
            // Add
            using (System.IO.StreamWriter file =
                 new System.IO.StreamWriter(@"imageMaster.txt", true))
            {
                file.Write($"\n{imageLink}");
            }

            // Add to in-progress
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"imageProgress.txt", true))
            {
                file.Write($"\n{imageLink}");
            }

            // Reload
            ImageHander.ReloadImages();
            ImageHander.ReloadImagesMasterList();
            await Context.Channel.SendMessageAsync("Image added");
        }

        [Command("count")]
        public async Task CountImages() => await Context.Channel.SendMessageAsync($"Total Images: {ImageHander.ImageMasterList.Count}");

        // See if the link is already existing
        [Command("find")]
        public async Task FindImage([Remainder] string link)
        {
            var sb = new StringBuilder();
            var i = 0;
            for (int ii = 0; ii < ImageHander.ImageMasterList.Count; ii++)
            {
                var s = ImageHander.ImageMasterList[ii];
                if (s.ToLower().Contains(link.ToLower()))
                    sb.Append($"{++i}. {s}\n");
            }

            // Embed
            await ReplyAsync("", false, new EmbedBuilder()
                .WithTitle("Images")
                .WithDescription(sb.ToString())
                .WithColor(GordonRamsayBot.Colours.Blue)
                .Build());
        }

        // delete image if it doesn't work
        [Command("delete")]
        public async Task DeleteImage([Remainder] string link)
        {
            var ip = false;
            var ml = false;
            for (int i = 0; i < ImageHander.images.Count; i++)
            {
                if (!ip && ImageHander.images[i].ToLower().Equals(link.ToLower()))
                {
                    ip = true;
                    ImageHander.images.RemoveAt(i);
                    ImageHander.RewriteImages();
                    await Context.Channel.SendMessageAsync($"Image deleted from list!");
                    break;
                }
            }

            for (int i = 0; i < ImageHander.ImageMasterList.Count; i++)
            {
                if (!ml && ImageHander.ImageMasterList[i].ToLower().Equals(link.ToLower()))
                {
                    ml = true;
                    ImageHander.ImageMasterList.RemoveAt(i);
                    ImageHander.RewriteMasterListImages();
                    await Context.Channel.SendMessageAsync($"Image deleted from master list.");
                    break;
                }
            }

            var sb = new StringBuilder();
            if (!ip || !ml)
                sb.Append("That image wasn't found" + (!ip && !ml ? " in either list." : ml ? " in the in-progress list." : " in the master list."));
            else
                sb.Append($"New image count:\nTotal: {ImageHander.ImageMasterList.Count}\nIn-Progress: {ImageHander.images.Count}");
            await Context.Channel.SendMessageAsync(sb.ToString());
        }
    }
}