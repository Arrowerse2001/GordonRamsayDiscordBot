using System.Collections.Generic;

namespace GordonRamsayBot
{
    static class ArrayHandler
    {
        static public string[] Insults =
        {
         "WAKE UP!",
         "YOU FUCKING DONKEY!",
         "Sounds like the weirdo on Dr. Phil!",
         "Oi, you're more laid back than a fucking iron board!",
         "That souffle has sunk so badly, James Cameron wants to make a film about it!",
         "WHAT ARE YOU! ||AN IDIOT SANDWICH!||",
         "I wish you'd jump in the oven, that'll make my life easier!",
         "You cook like old people fuck!",
         "I have never, ever, ever met someone I believe in as little as you!",
         "You surprise me.... You surprise me by how shit you are!",
         "GET OUT! <a:GetOut:811398024696692736>",
         "You derseve a kick in the nuts!",
         "You look like you're just about to lose your virginity.",
         "Now fuck off you fat useless sack of fucking yanke doodle dandy shite. Fuck off will ya.",
         "Piss off!",
         "Your crabs are so undercooked, you can give them back to your girlfriend!",
         "You're a fucking disgrace of a chef, AT LEAST HANNIBAL KNEW HOW TO SERVER PEOPLE!",
         "YOUR CAKE IS SO DARK AND RICH A FUCKING KARDASHIAN WANTS TO MARRY IT!",
         "YOUR FISH IS SO RAW IT'S STARTING TO SWIM IN THE BLOODY SOUP!",
         "I wouldn't trust you to run a bath let alone a fucking restaurant!",
         ", You add so much salt and pepper to your food I can hear your food singing Push it from here!"
        };

        static public string[] GordonImages =
        {
            "https://cdn.discordapp.com/attachments/809313937031168040/809529271123050587/WH8G.gif",
            "https://giphy.com/gifs/bfd-l0GRkZMMmYYHoUVbi",
            "https://tenor.com/view/gordon-ramsay-you-are-a-fucking-disgrace-disgrace-pissed-mad-gif-4618433",
            "https://cdn.discordapp.com/emojis/811430086796116020.gif?v=1",
            "https://tenor.com/view/wow-excited-gordon-ramsay-gif-7527763",
            "https://tenor.com/view/wow-excited-gordon-ramsay-gif-7527763",
            "https://tenor.com/view/gordon-ramsay-master-chef-weirdo-gif-14751321",
            "https://tenor.com/view/gordon-ramsay-chef-ramsay-gif-19590217",
            "https://c.tenor.com/p3r4gLkn3-gAAAAM/laughing-happy.gif",
            "https://media.giphy.com/media/1BFG6qI7KatMo7unP2/giphy.gif",
            "https://tenor.com/view/finesse-class-where-gordon-ramsay-pissed-gif-6149378",
            ""
        };

        // strings for files
        static readonly string MasterPath = @"MasterList.txt";
        static readonly string insultP = @"insultsProgress.txt";
        static public List<string> MasterList = new List<string>();
        static public List<string> gInsults = new List<string>();

        static ArrayHandler()
        {
            ReloadInsults();
            ReloadMasterList();
        }

        // Reload Insults so i don't need to restart every time
        static public void ReloadInsults()
        {
            gInsults.Clear();
            gInsults = new List<string>(System.IO.File.ReadAllLines(insultP));
            // No blanks
            for (int i = 0; i < gInsults.Count; i++)
                if (gInsults[i].Length < 1) gInsults.RemoveAt(i);
        }

        // Reload a master file with all of the insults
        static public void ReloadMasterList()
        {
            MasterList.Clear();
            MasterList = new List<string>(System.IO.File.ReadAllLines(MasterPath));
            // No blanks
            for (int i = 0; i < MasterList.Count; i++)
                if (MasterList[i].Length < 1) MasterList.RemoveAt(i);
        }

        // Rewrite to Insults
        static public void RewriteInsults()
        {
            System.IO.File.WriteAllLines(insultP, gInsults.ToArray());
            ReloadInsults();
        }

        // Reload Master List
        static public void RewriteMasterListInsults()
        {
            System.IO.File.WriteAllLines(MasterPath, MasterList.ToArray());
            ReloadMasterList();
        }

        // Add from Master to In progress so it doesn't repeat often
        static public void AddAllMasterInsultsToProgressList()
        {
            gInsults.Clear();
            gInsults = new List<string>(System.IO.File.ReadAllLines(MasterPath));
            System.IO.File.WriteAllLines(insultP, gInsults.ToArray());

            for (int i = 0; i < gInsults.Count; i++)
                if (gInsults[i].Length < 1) gInsults.RemoveAt(i);
        }

        // Get Insult
        static public string GetInsult(int index)
        {
            string i = gInsults[index];
            gInsults.RemoveAt(index);
            if (gInsults.Count == 0)
                AddAllMasterInsultsToProgressList();
            else
                RewriteInsults();
            return i;
        }
    }
}