using System;
using Discord;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GordonRamsayBot.Commands
{

    [RequireContext(ContextType.Guild)]
    public class Misc : ModuleBase<SocketCommandContext>
    {
        // Ping to see if the bot is on and will reply back
        [Command("ping")]
        public async Task PingPong() => await Context.Channel.SendMessageAsync($"Pong!");

        // Insult someone by using a Gordon Ramsay Quote
        [Command("insult")]
        public async Task GordonInsult(SocketGuildUser user = null)
        {
            if (user == null)
                user = (SocketGuildUser)Context.User;
            int iN = Utilities.GetRandomNumber(0, ArrayHandler.gInsults.Count);
            var insult = ArrayHandler.GetInsult(iN);
            await Context.Channel.SendMessageAsync($"{user.Mention} {insult}");
        }

        // Prints an image or gif of Gordon
        [Command("image")]
        public async Task SendGordonImage()
        {
            int imageNum = Utilities.GetRandomNumber(0, GordonRamsayDiscordBot.Handlers.ImageHander.images.Count);
            var image = GordonRamsayDiscordBot.Handlers.ImageHander.GetGordonImage(imageNum);
            await Utilities.SendDomColorEmbed(Context.Channel, "", image, image, "");
        }

        // trivia
        [Command("trivia")]
        public async Task TryToStartTrivia(string input = null) => await Handlers.MinigameHandler.Trivia.TryToStartTrivia((SocketGuildUser)Context.User, Context, input ?? "trivia");
    
        // Bio
        [Command("bio")]
        public async Task DisplayGordonBio()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Gordon Ramsay")
                .WithDescription("Gordon James Ramsay OBE is a British chef, restaurateur, television personality, and writer. Born in Johnstone, Scotland, and raised in Stratford-upon-Avon, England, he founded his global restaurant group, Gordon Ramsay Restaurants, in 1997.")
                .WithFooter("Source: Wikipedia")
                .WithColor(Colours.Blue)
                .WithThumbnailUrl("http://www.gstatic.com/tv/thumb/persons/319794/319794_v9_bb.jpg")
                .WithUrl("https://www.gordonramsay.com")
                .Build();
            await ReplyAsync("", false, builder.Build());
        }

        // suggest
        [Command("suggest")]
        public async Task SuggestSomethingPingArrow([Remainder]string suggestion)
        {
            var client = Context.Client;
            ulong channelID = 728918609027661828;
            var c = client.GetChannel(channelID) as SocketTextChannel;
            await c.SendMessageAsync($"<@470033984575897600> New suggestion: {suggestion}\nFrom: {Context.User.Mention}");
            await Context.Channel.SendMessageAsync($"Your suggestion: ``{suggestion}`` has been sent to the bot developer.");
        }

        //help menu
        [Command("help")]
        public async Task DisplayHelp()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Help!")
                .WithColor(Colours.Blue)
                .AddField("gr!suggest", "Submit something you want in the bot")
                .AddField("gr!bio", "Displays Gordon's Bio")
                .AddField("gr!insult @user", "Use a famous line to insult someone!")
                .AddField("gr!image", "Displays a random image of Gordon")
                .AddField("gr!github", "Get a link to the bot github page")
                .AddField("gr!trivia", "Shows trivia menu")
                .AddField("gr!trivia solo", "Play trivia solo")
                .AddField("gr!trivia all", "First to answer wins")
                .AddField("gr!stats", "Display Bot Stats");
            builder.Build();
            await ReplyAsync("", false, builder.Build());
        }

        // GitHub
        [Command("github")]
        public async Task ShowGithub() => await Context.Channel.SendMessageAsync("https://github.com/Arrowerse2001/GordonRamsayDiscordBot");

        // My stats to see how many servers my bot is in
        [Command("stats")]
        public async Task GetServers() => await Utilities.DisplayBotStats(Context.Channel, Context.Client);

        // Reset trivia
        [Command("reset trivia")]
        public async Task ResetTrivia() => await GordonRamsayBot.Handlers.MinigameHandler.ResetGame(Context, "trivia");
        
    }
}
