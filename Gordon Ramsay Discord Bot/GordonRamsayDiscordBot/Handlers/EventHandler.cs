using System;
using Discord;
using System.Linq;
using Discord.Commands;
using System.Reflection;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace GordonRamsayBot.Handlers
{
    class EventHandler
    {
        DiscordSocketClient _client;
        CommandService _service;
        readonly IServiceProvider serviceProdiver;

        public EventHandler(IServiceProvider services) => serviceProdiver = services;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();


            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProdiver);

            _client.MessageReceived += HandleCommandAsync;



            _service.Log += Log;

        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }


        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = s as SocketUserMessage;
            if (msg == null || msg.Author.IsBot) return;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasStringPrefix("gr!", ref argPos))
                await _service.ExecuteAsync(context, argPos, serviceProdiver, MultiMatchHandling.Exception);
           if (msg.HasStringPrefix("!gr", ref argPos))
                await _service.ExecuteAsync(context, argPos, serviceProdiver, MultiMatchHandling.Exception);

            string m = msg.Content.ToLower();

            if (m.Contains("donkey"))
            {
                await context.Channel.SendMessageAsync($"{context.User.Mention} YOU FUCKING DONKEY!");
                await context.Channel.SendMessageAsync($"<a:YouFuckingDonkey:811390524656189500>");
            }

            if (m == "a" || m == "b" || m == "c" || m == "d")
                await MinigameHandler.Trivia.AnswerTrivia((SocketGuildUser)msg.Author, context, m);
        }
    }
}