namespace NET23_GrupprojektBank.Managers.UserInteraction
{
    internal static class HowToUpdateCurrency
    {
        public static int AdminUpdateCurrencyOption()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("How would you like to update the currency exchange rate?");
                Console.WriteLine("1). From File.");
                Console.WriteLine("2). From The Intrawebbs.");
                Console.WriteLine();
                Console.WriteLine("0). To Go Back.");
                Console.Write("Choice: ");
                string userChoice = Console.ReadLine().ToUpper();
                switch (userChoice)
                {
                    case "1":
                    case "FILE":
                        return 1;
                    case "2":
                    case "INTERNET":
                    case "INTRAWEBBS":
                        return 2;
                    case "0":
                    case "RETURN":
                    case "BACK":
                        return 0;
                    default:
                        Console.WriteLine("Please choose either 1 or 2");
                        Console.WriteLine("Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }



    }
}
