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
                
                switch (stringChoice)
                {
                    case "Email":
                        email = UserCommunications.GetEmailFromUser();
                        choices.Remove("Email");
                        Console.Clear();
                        switch (email.EmailAddress)
                        {
                            case "-1":
                                AddLog(EventStatus.EmailFailed);
                                if (!UserCommunications.AskUserYesOrNo("Continue account creation without an email?"))
                                {
                                    StopUserCreation("Admin creation cancelled", EventStatus.AdminAccountCreationFailed);
                                    return null;
                                }
                                break;
                            default:
                                AddLog(EventStatus.EmailSuccess);
                                break;
                        }
                        break;

                    case "Phone":
                        phone = UserCommunications.GetPhoneFromUser();
                        choices.Remove("Phone");
                        Console.Clear();
                        switch (phone.PhoneNumber)
                        {
                            case "-1":
                                AddLog(EventStatus.PhoneFailed);
                                if (!UserCommunications.AskUserYesOrNo("Continue account creation without a phone number?"))
                                {
                                    StopUserCreation("Admin creation cancelled", EventStatus.AdminAccountCreationFailed);
                                    return null;
                                }
                                break;
                            default:
                                AddLog(EventStatus.PhoneSuccess);
                                break;
                        }
                        break;

                    case "Adress":
                        adress = UserCommunications.GetAdressFromUser();
                        choices.Remove("Adress");
                        Console.Clear();
                        switch (adress.Country)
                        {
                            case "-1":
                                AddLog(EventStatus.AdressFailed);
                                if (!UserCommunications.AskUserYesOrNo("Continue account creation without an adress?"))
                                {
                                    StopUserCreation("Admin creation cancelled", EventStatus.AdminAccountCreationFailed);
                                    return null;
                                }
                                break;
                            default:
                                AddLog(EventStatus.AdressSuccess);
                                break;
                        }
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
                
                switch (stringChoice)
                {
                    case "Email":
                        email = UserCommunications.GetEmailFromUser();
                        choices.Remove("Email");
                        Console.Clear();
                        switch (email.EmailAddress)
                        {
                            case "-1":
                                AddLog(EventStatus.EmailFailed);
                                if (!UserCommunications.AskUserYesOrNo("Continue account creation without an email?"))
                                {
                                    StopUserCreation("Customer creation cancelled", EventStatus.CustomerAccountCreationFailed);
                                    return null;
                                }
                                break;
                            default:
                                AddLog(EventStatus.EmailSuccess);
                                break;
                        }
                        break;

                    case "Phone":
                        phone = UserCommunications.GetPhoneFromUser();
                        choices.Remove("Phone");
                        Console.Clear();
                        switch (phone.PhoneNumber)
                        {
                            case "-1":
                                AddLog(EventStatus.PhoneFailed);
                                if (!UserCommunications.AskUserYesOrNo("Continue account creation without a phone number?"))
                                {
                                    StopUserCreation("Customer creation cancelled", EventStatus.CustomerAccountCreationFailed);
                                    return null;
                                }
                                break;
                            default:
                                AddLog(EventStatus.PhoneSuccess);
                                break;
                        }
                        break;

                    case "Adress":
                        adress = UserCommunications.GetAdressFromUser();
                        choices.Remove("Adress");
                        Console.Clear();
                        switch (adress.Country)
                        {
                            case "-1":
                                AddLog(EventStatus.AdressFailed);
                                if (!UserCommunications.AskUserYesOrNo("Continue account creation without an adress?"))
                                {
                                    StopUserCreation("Customer creation cancelled", EventStatus.CustomerAccountCreationFailed);
                                    return null;
                                }
                                break;
                            default:
                                AddLog(EventStatus.AdressSuccess);
                                break;
                        }
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
