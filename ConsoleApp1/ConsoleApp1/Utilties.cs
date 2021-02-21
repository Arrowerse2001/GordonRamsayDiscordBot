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


    }
}