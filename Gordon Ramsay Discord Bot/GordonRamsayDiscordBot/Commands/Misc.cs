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
            await Context.Channel.SendMessageAsync($"{user.Mention} {ArrayHandler.Insults[Utilities.GetRandomNumber(0, ArrayHandler.Insults.Length)]}");
        }

        // Prints an image or gif of Gordon
        [Command("image")]
        public async Task SendGordonImage()
        {
            string image = ArrayHandler.GordonImages[Utilities.GetRandomNumber(0, ArrayHandler.GordonImages.Length)];
            await Context.Channel.SendMessageAsync("", false, Utilities.ImageEmbed("", "", Colours.Blue, "", image));
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
                .AddField("!suggest", "Submit something you want in the bot")
                .AddField("!bio", "Displays Gordon's Bio")
                .AddField("!insult @user", "Use a famous line to insult someone!")
                .AddField("!image", "Displays a random image of Gordon")
                .AddField("!github", "Get a link to the bot github page")
                .AddField("!trivia", "Shows trivia menu")
                .AddField("!trivia solo", "Play trivia solo")
                .AddField("!trivia all", "First to answer wins")
                .AddField("!stats", "Display Bot Stats");
            builder.Build();
            await ReplyAsync("", false, builder.Build());
        }

        // GitHub
        [Command("github")]
        public async Task ShowGithub() => await Context.Channel.SendMessageAsync("https://github.com/Arrowerse2001/GordonRamsayDiscordBot");

        // My stats to see how many servers my bot is in
        [Command("stats")]
        public async Task GetServers() => await Utilities.DisplayBotStats(Context.Channel, Context.Client);
        
    }
}
