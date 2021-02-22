using System;
using Discord;
using System.IO;
using System.Net;
using System.Drawing;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace GordonRamsayBot
{
    static class Utilities
    {

        // Get a random number
        public static readonly Random getrandom = new Random();
        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) { return getrandom.Next(min, max); }
        }

        // Generic Embed template
        public static Embed Embed(string title, string desc, Discord.Color col, string foot, string thURL) => new EmbedBuilder()
            .WithTitle(title)
            .WithDescription(desc)
            .WithColor(col)
            .WithFooter(foot)
            .WithThumbnailUrl(thURL)
            .Build();

        // Generic Image Embed template
        public static Embed ImageEmbed(string t, string d, Discord.Color c, string f, string imageURL) => new EmbedBuilder()
            .WithTitle(t)
            .WithDescription(d)
            .WithColor(c)
            .WithFooter(f)
            .WithImageUrl(imageURL)
            .Build();

        // Print a success message
        public static async Task PrintSuccess(ISocketMessageChannel channel, string description) => await SendEmbed(channel, "Success", description, Colours.Green, "", "").ConfigureAwait(false);

        // Print an error
        public static async Task PrintError(ISocketMessageChannel channel, string description) => await SendEmbed(channel, "Error", description, Colours.Red, "", "").ConfigureAwait(false);


        // Send an embed to a channel
        public static async Task SendEmbed(ISocketMessageChannel channel, string title, string description, Discord.Color color, string footer, string thumbnailURL)
        {
            await channel.SendMessageAsync(null, false, Embed(title, description, color, footer, thumbnailURL)).ConfigureAwait(false);
        }

        // Bot Stats
        public static async Task DisplayBotStats(this ISocketMessageChannel Channel, DiscordSocketClient Client)
        {
            await Channel.SendMessageAsync(null, false, new EmbedBuilder()
                .WithTitle("Bot Info")
                .WithColor(Colours.Blue)
                .AddField("Servers", Client.Guilds.Count)
                .AddField("Developer", "Arrowverse2001")
                .AddField("Links", "[Invite](https://discord.com/api/oauth2/authorize?client_id=487596701947789322&permissions=67501120&scope=bot) | [GitHub](https://github.com/Arrowerse2001/GordonRamsayDiscordBot) | [Support Server](https://discord.gg/X7na2Sx)")
                .Build()).ConfigureAwait(false);
        }
    }
}