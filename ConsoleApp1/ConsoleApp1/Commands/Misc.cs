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
    }
}
