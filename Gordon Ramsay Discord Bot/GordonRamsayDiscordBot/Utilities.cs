using System;
using Discord;
using System.IO;
using System.Net;
using System.Drawing;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Drawing;
using ColorThiefDotNet;
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

        // Error Message
        public static async Task PrintError(ISocketMessageChannel channel, string description) => await SendEmbed(channel, "Error", description, Colours.Red, "", "").ConfigureAwait(false);


        // Embed
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
                .AddField("Developer", "Arrowverse2001#2001")
                .AddField("Links", "[Invite](https://discord.com/api/oauth2/authorize?client_id=487596701947789322&permissions=67501120&scope=bot) | [GitHub](https://github.com/Arrowerse2001/GordonRamsayDiscordBot) | [Support Server](https://discord.gg/X7na2Sx)")
                .Build()).ConfigureAwait(false);
        }

        // Thanks Reverse :D
        public static Discord.Color HexToRGB(string hex)
        {
            // First two values of the hex
            int r = int.Parse(hex.Substring(0, hex.Length - 4), System.Globalization.NumberStyles.AllowHexSpecifier);

            // Get the middle two values of the hex
            int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            // Final two values
            int b = int.Parse(hex.Substring(4), System.Globalization.NumberStyles.AllowHexSpecifier);

            return new Discord.Color(r, g, b);
        }

        // Dom colour
        private static ColorThief colorThief = new ColorThief();
        public static readonly WebClient webClient = new WebClient();
        public static Discord.Color DomColorFromURL(string url)
        {
            byte[] bytes = webClient.DownloadData(url);
            using (webClient)
            using (MemoryStream ms = new MemoryStream(bytes))
            using (Bitmap bitmap = new Bitmap(System.Drawing.Image.FromStream(ms)))
            {
                // Remove the '#' from the string and get the hexadecimal
                return HexToRGB(colorThief.GetColor(bitmap).Color.ToString().Substring(1));
            }
        }

        // Embed
        public static async Task SendDomColorEmbed(ISocketMessageChannel channel, string title, string description, string imageURL, string footer = null)
        {
            await channel.SendMessageAsync(null, false, Embed(title, description, DomColorFromURL(imageURL), footer, imageURL)).ConfigureAwait(false);
        }

    }
}