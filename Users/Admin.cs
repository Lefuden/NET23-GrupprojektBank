using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users.UserContactInformation;
using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;
using Spectre.Console;

namespace NET23_GrupprojektBank.Users
{
    internal class Admin : User
    {
        public Admin(string username, string password, PersonInformation person) : base(username, password, person)
        {
            UserType = UserType.Admin;
        }

        public Admin CreateAdminAccount(List<string> existingUsernames)
        {
            Email email = new("","");
            Phone phone = new("");
            Address adress = new("","","","");
            List<string> choices = new()
            {
                "Email",
                "Phone",
                "Adress",
                "Complete",
                "Back"
            };

            var (username, password, firstName, lastName, dateOfBirth) = UserCommunications.GetBasicsFromUser(existingUsernames);
            if (username == "-1")
            {
                StopUserCreation("Admin creation cancelled", EventStatus.AdminAccountCreationFailed);
                return null;
            }

            while (true)
            {
                string stringChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title($"[purple]Select field[/]")
                    .PageSize(5)
                    .AddChoices(choices));
                string switchArgument;
                switch (stringChoice)
                {
                    case "Email":
                        choices.Remove("Email");
                        (email, switchArgument) = UserCommunications.GetEmailFromUser();
                        if (CancelOrProceedUserCreation(stringChoice, "Admin", switchArgument)) return null;
                        break;

                    case "Phone":
                        choices.Remove("Phone");
                        (phone, switchArgument) = UserCommunications.GetPhoneFromUser();
                        if (CancelOrProceedUserCreation(stringChoice, "Admin", switchArgument)) return null;
                        break;

                    case "Adress":
                        choices.Remove("Adress");
                        (adress, switchArgument) = UserCommunications.GetAdressFromUser();
                        if (CancelOrProceedUserCreation(stringChoice, "Admin", switchArgument)) return null;
                        break;

                    case "Complete":
                        StopUserCreation("New admin has been created", EventStatus.AdminAccountCreationSuccess);
                        return new Admin(username, password,
                               new PersonInformation(firstName, lastName, dateOfBirth,
                               new ContactInformation(email, phone, adress)));
                    
                    case "Back":
                        StopUserCreation("Admin creation cancelled", EventStatus.AdminAccountCreationFailed);
                        return null;
                }

                if (choices.Count == 2)
                {
                    StopUserCreation("New admin has been created", EventStatus.AdminAccountCreationSuccess);
                    return new Admin(username, password,
                           new PersonInformation(firstName, lastName, dateOfBirth,
                           new ContactInformation(email, phone, adress)));
                }
            }
        }

        public Customer CreateCustomerAccount(List<string> existingUsernames)
        {
            Email email = new("","");
            Phone phone = new("");
            Address adress = new("","","","");
            List<string> choices = new()
            {
                "Email",
                "Phone",
                "Adress",
                "Complete",
                "Back"
            };

            var (username, password, firstName, lastName, dateOfBirth) = UserCommunications.GetBasicsFromUser(existingUsernames);
            if (username == "-1")
            {
                StopUserCreation("Customer creation cancelled", EventStatus.CustomerAccountCreationFailed);
                return null;
            }

            while (true)
            {
                string stringChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title($"[purple]Select field[/]")
                    .PageSize(5)
                    .AddChoices(choices));

                string switchArgument;
                switch (stringChoice)
                {
                    case "Email":
                        choices.Remove("Email");
                        (email, switchArgument) = UserCommunications.GetEmailFromUser();
                        if (CancelOrProceedUserCreation(stringChoice, "Customer", switchArgument)) return null;
                        break;

                    case "Phone":
                        choices.Remove("Phone");
                        (phone, switchArgument) = UserCommunications.GetPhoneFromUser();
                        if (CancelOrProceedUserCreation(stringChoice, "Customer", switchArgument)) return null;
                        break;

                    case "Adress":
                        choices.Remove("Adress");
                        (adress, switchArgument) = UserCommunications.GetAdressFromUser();
                        if (CancelOrProceedUserCreation(stringChoice, "Customer", switchArgument)) return null;
                        break;

                    case "Complete":
                        StopUserCreation("New customer has been created", EventStatus.CustomerAccountCreationSuccess);
                        return new Customer(username, password,
                               new PersonInformation(firstName, lastName, dateOfBirth,
                               new ContactInformation(email, phone, adress)));
                    
                    case "Back":
                        StopUserCreation("Customer creation cancelled", EventStatus.CustomerAccountCreationFailed);
                        return null;
                }

                if (choices.Count == 2)
                {
                    StopUserCreation("New customer has been created", EventStatus.CustomerAccountCreationSuccess);
                    return new Customer(username, password,
                           new PersonInformation(firstName, lastName, dateOfBirth,
                           new ContactInformation(email, phone, adress)));
                }
            }
        }

        public bool CancelOrProceedUserCreation(string selectedChoice, string adminOrCustomer, string switchArgument)
        {
            Console.Clear();
            switch (switchArgument)
            {
                case "-1":
                    if (Enum.TryParse($"{selectedChoice}Failed", out EventStatus eventFailed))
                    {
                        AddLog(eventFailed);
                    }
                    else
                    {
                        AddLog(EventStatus.InvalidInput);
                    }
                    

                    if (!UserCommunications.AskUserYesOrNo($"Continue account creation without {selectedChoice}?"))
                    {
                        if (Enum.TryParse($"{adminOrCustomer}AccountCreationFailed", out EventStatus userFailed))
                        {
                            StopUserCreation($"{adminOrCustomer} creation cancelled", userFailed);
                            return true;
                        }
                        AddLog(EventStatus.InvalidInput);
                    }
                    break;
                default:
                    if (Enum.TryParse($"{selectedChoice}Success", out EventStatus eventSuccess))
                    {
                        AddLog(eventSuccess);
                        break;
                    }
                    AddLog(EventStatus.InvalidInput);
                    break;
            }
            return false;
        }
        public void StopUserCreation(string message, EventStatus status)
        {
            AnsiConsole.MarkupLine($"[green]{message}[/]");
            AddLog(status);
            UserCommunications.FakeBackChoice("Ok");
        }
        public void UpdateCurrencyExchangeRate()
        {
            EventStatus taskUpdateStatus = CurrencyExchangeRate.UpdateCurrencyExchangeRateAsync(UserType).Result;
            AddLog(taskUpdateStatus);
        }
    }
}
