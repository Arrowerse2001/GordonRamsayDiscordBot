using System;
using GordonRamsayBot.Minigames;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GordonRamsayBot.Handlers
{
    class MinigameHandler
    {
        public static Trivia Trivia = new Trivia();
        public static TriviaQuestions TriviaQuestions;






        // Set up Trivia Questions
        public static void InitialTriviaSetup()
        {
            TriviaQuestions = JsonConvert.DeserializeObject<TriviaQuestions>(System.IO.File.ReadAllText(@"C:\Users\IainN\OneDrive\Desktop\Bot\GordonRamsayDiscordBot\ConsoleApp1\ConsoleApp1\Minigames\trivia_questions.json"));
        }

        

        public async Task TryToStartTrivia(SocketCommandContext context, string input)
        {
            if (input == "all")
                await Trivia.TryToStartTrivia((SocketGuildUser)context.User, context, "all");
        }


        // Reset a game
        public static async Task ResetGame(SocketCommandContext context, string game)
        {
           if (game == "trivia")
            {
                await Utilities.SendEmbed(context.Channel, "MiniGames", $"{context.User.Mention} has reset Trivia.", Colours.Green, "", "");
                Trivia.ResetTrivia();
            }


            else if (game == "")
                await Utilities.PrintError(context.Channel, "Please specify a game to reset.");
        }
    }


    // Trivia Questions
    public partial class TriviaQuestions
    {
        [JsonProperty("Questions")]
        public TriviaQuestion[] Questions { get; set; }
    }

    public class TriviaQuestion
    {
        [JsonProperty("Question")]
        public string QuestionQuestion { get; set; }

        [JsonProperty("Answer")]
        public string Answer { get; set; }

        [JsonProperty("IncorrectAnswers")]
        public string[] IncorrectAnswers { get; set; }
    }
}