using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

            EmbedBuilder b = new EmbedBuilder();
            b.WithImageUrl(image)
                .WithColor(Colours.Blue);
            b.Build();
            await ReplyAsync("", false, b.Build());
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
                .AddField("gr!serverstats", "Show server stats")
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

        // Server Stats
        [Command("serverstats")]
        [Alias("server stats", "serverinfo", "server info")]
        public async Task DisplaySeverStats()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle(Context.Guild.Name);
            builder.AddField("Created", Context.Guild.CreatedAt.ToString("dddd, MMMM d, yyyy"));
            builder.AddField("Owner", Context.Guild.Owner.Mention);
            builder.AddField("Emotes", Context.Guild.Emotes.Count);
            builder.AddField("Members", Context.Guild.MemberCount.ToString("#,##0"));
            builder.WithThumbnailUrl(Context.Guild.IconUrl);
            builder.WithColor(Colours.Blue);
            builder.Build();
            await ReplyAsync("", false, builder.Build());
        }
        
        // Get user avatar
        [Command("avatar")]
        [Alias("av", "avi")]
        public async Task GetUserAvatar(SocketGuildUser user = null)
        {
            if (user == null)
                user = (SocketGuildUser)Context.User;
            await Context.Channel.SendMessageAsync("", false, Utilities.ImageEmbed($"{user.Nickname ?? user.Username}'s Avatar", "", Colours.Blue, "", user.GetAvatarUrl()));
        }

        // Find People with the desired role
        private string GetMembersWithRole(string DesiredRole)
        {
            SocketRole role = Context.Guild.Roles.FirstOrDefault(x => x.Name == DesiredRole);
            StringBuilder d = new StringBuilder();
            foreach (SocketGuildUser user in Context.Guild.Users.ToArray())
            {
                if (user.Roles.Contains(role))
                    d.AppendLine($"{user.Mention}");
            }
            return d.ToString();
        }

        // Command to see who has the role (role is case sensitive)
        [Command("members")]
        public async Task FindPeopleInRoles([Remainder]string role) => await Utilities.SendEmbed(Context.Channel, $"Members", GetMembersWithRole(role), Colours.Blue, "", "");

        // Reply to a message
        [Command("reply")]
        public async Task Reply(ISocketMessageChannel channel, ulong msg, [Remainder] string reply)
        {
            SocketGuildUser cu = (SocketGuildUser)Context.User;
            if (cu.GuildPermissions.ManageMessages == false)
            {
                await Context.Channel.SendMessageAsync($"I know you may be slightly stupid but you do not have permission to use this command!");
            }
            else
            {
                var client = Context.Client;
                ulong channelID = channel.Id;
                var c = client.GetChannel(channelID) as SocketTextChannel;

                MessageReference m = new MessageReference(msg);

                await c.SendMessageAsync(reply, false, null, null, null, m);
            }
        }

        // Display server stats
        [Command("severstats")]
        [Alias("server stats", "serverinfo", "server info")]
        public async Task DisplayServerStats()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"{Context.Guild.Name}'s Server Stats")
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithColor(Colours.Blue)
                .AddField("Created", Context.Guild.CreatedAt.ToString("dddd, MMMM d, yyyy"))
                .AddField("Owner: ", $"{Context.Guild.Owner.Mention}")
                .AddField("Emotes: ", $"{Context.Guild.Emotes.Count}")
                .AddField("Members: ", $"{Context.Guild.MemberCount}");
            builder.Build();
            await ReplyAsync("", false, builder.Build());
        }

        // Say something to a specific channel
        [Command("say")]
        public async Task SaySomething(ISocketMessageChannel channel, [Remainder] string msg)
        {
            SocketGuildUser cu = (SocketGuildUser)Context.User;
            if (cu.GuildPermissions.ManageMessages == false)
            {
                await Context.Channel.SendMessageAsync($"I know you may be slightly stupid but you do not have permission to use this command!");
            }
            else
            {
                var client = Context.Client;
                ulong channelID = channel.Id;
                var c = client.GetChannel(channelID) as SocketTextChannel;
                await c.SendMessageAsync($"{msg}");
                await Context.Channel.SendMessageAsync($"Posted");
            }
        }

        // Mod Commands
        // Ban
        [Command("ban")]
        public async Task Ban(SocketGuildUser user, [Remainder] string reason = null)
        {
            var contextUser = (SocketGuildUser)Context.User;
            if (contextUser.GuildPermissions.BanMembers == false)
            {
                await Context.Channel.SendMessageAsync($"You don't have that permission you fucking doughnut!");
            }
            else if (reason == null)
            {
                await Context.Channel.SendMessageAsync($"You need to provide a reason.");
            }
            else if (contextUser.GuildPermissions.BanMembers == true)
            {
                foreach (var channel in Context.Guild.Channels)
                {
                    if (channel.Name == "logs" || channel.Name == "log")
                    {
                        var client = Context.Client;
                        ulong channelID = channel.Id;
                        var c = client.GetChannel(channelID) as SocketTextChannel;

                        EmbedBuilder builder = new EmbedBuilder();
                        builder.WithTitle("Member Banned")
                            .WithThumbnailUrl(user.GetAvatarUrl())
                            .WithColor(Colours.Red)
                            .AddField("User Banned: ", $"{user}")
                            .AddField("By: ", $"{Context.User.Mention}")
                            .AddField("Reason: ", $"{reason}");
                        builder.Build();
                        await c.SendMessageAsync("", false, builder.Build());
                        break;
                    }
                }
                await user.SendMessageAsync($"You have been banned!\nServer: {Context.Guild.Name}\nBy: {Context.User}\nReason: {reason}");
                await Context.Guild.AddBanAsync(user, 0, reason, null);
                await Context.Channel.SendMessageAsync($"{user} has been banned by {Context.User.Mention} for {reason}.");
            }
        }

        // Kick
        [Command("kick")]
        public async Task KickMembers(SocketGuildUser user, [Remainder] string reason)
        {
            var contextUser = (SocketGuildUser)Context.User;
            if (contextUser.GuildPermissions.KickMembers == false)
            {
                await Context.Channel.SendMessageAsync($"You don't have permission to do that you fucking idiot!");
            }
            else if (contextUser.GuildPermissions.KickMembers == true)
            {
                await user.KickAsync(reason);
                await Context.Channel.SendMessageAsync($"{user} has been kicked by {Context.User} for {reason}");
            }
        }

        // Purge
        [Command("delete")]
        [Alias("purge", "clean", "clear", "remove")]
        public async Task PurgeMessages(int amount)
        {
            var contextUser = (SocketGuildUser)Context.User;

            if (contextUser.GuildPermissions.ManageMessages == false)
            {
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} You fucking idiot, you do not have that permission!");
                await Context.Channel.SendMessageAsync($"<a:GordonPathetic:811434817404272700>");
            }
            else if (contextUser.GuildPermissions.ManageChannels == true)
            {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
                await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
                IUserMessage m = await ReplyAsync($"Deleted {amount} message for you.");
                await Task.Delay(3000);
                await m.DeleteAsync();
            }
        }

        // Create Member Count Voice Channel
        [Command("create channel")]
        public async Task CreateChannel()
        {
            var contextUser = (SocketGuildUser)Context.User;

            if (contextUser.GuildPermissions.ManageChannels == false)
            {
                await Context.Channel.SendMessageAsync($"What an idiot. Trying to create a channel for a server he doesn't have any perms in.");
                await Context.Channel.SendMessageAsync($"<a:GordonPathetic:811434817404272700>");
            }
            else if (contextUser.GuildPermissions.ManageChannels == true)
            {
                await Context.Guild.CreateVoiceChannelAsync($"Members: {Context.Guild.MemberCount}");
            }
        }
    }
}
