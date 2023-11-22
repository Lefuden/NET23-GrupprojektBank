using Spectre.Console;
using System.Text.RegularExpressions;

namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal partial class UserCommunications
    {

        public static bool AskUserYesOrNo(string message)
        {
            string stringChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"[purple]{message}[/]")
                .PageSize(3)
                .AddChoices(new[]
                    {
                        "Yes",
                        "No"
                    }
                ));
            return stringChoice == "Yes";
        }



        public static bool UserAgeRestriction(DateTime userInput)
        {
            int userAge = DateTime.Now.Year - userInput.Year;
            return userAge >= 18;


        }
        private static int GetSingleMatch(string pattern, string input)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);

            if (match.Success)
            {
                // The captured number is in the first capturing group (index 1).
                if (int.TryParse(match.Groups[1].Value, out int convertedValue))
                {
                    return convertedValue;
                }

                return -1;
            }

            return -1;
        }
    }
}
