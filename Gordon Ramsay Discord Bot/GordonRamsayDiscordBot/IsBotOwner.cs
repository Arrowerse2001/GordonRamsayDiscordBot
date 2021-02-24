using System;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace GordonRamsayDiscordBot
{
    /// <summary>
    /// Make these commands only possible for the developer of this bot
    /// </summary>
    public class IsBotOwner : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.User is SocketGuildUser)
            {
                if (context.User.Id != 470033984575897600) // Arrowverse2001#2001
                    return await Task.FromResult(PreconditionResult.FromError("You do not have permission to perform this command.")).ConfigureAwait(false);
                return await Task.FromResult(PreconditionResult.FromSuccess()).ConfigureAwait(false);
            }

            return await Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command.")).ConfigureAwait(false);
        }
    }
}
