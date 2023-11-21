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
                StopUserCreation("Admin creation cancelled", EventStatus.AccountCreationFailed);
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
                        if (email.EmailAddress == "-1")
                        {
                            choices.Remove("Email");
                            Console.Clear();
                            if (!UserCommunications.AskUserYesOrNo("Continue account creation without email?"))
                            {
                                StopUserCreation("Admin creation cancelled", EventStatus.AccountCreationFailed);
                                return null;
                            }
                        }
                        else if (email != null)
                        {
                            choices.Remove("Email");
                        }
                        break;

                    case "Phone":
                        phone = UserCommunications.GetPhoneFromUser();
                        if (phone.PhoneNumber == "-1")
                        {
                            Console.Clear();
                            if (!UserCommunications.AskUserYesOrNo("Continue account creation without phone number?"))
                            {
                                StopUserCreation("Admin creation cancelled", EventStatus.AccountCreationFailed);
                                return null;
                            }
                        }
                        else if (phone != null)
                        {
                            choices.Remove("Phone");
                        }
                        break;

                    case "Adress":
                        adress = UserCommunications.GetAdressFromUser();
                        if (adress.Country == "-1")
                        {
                            choices.Remove("Adress");
                            Console.Clear();
                            if (!UserCommunications.AskUserYesOrNo("Continue account creation without adress?"))
                            {
                                StopUserCreation("Admin creation cancelled", EventStatus.AccountCreationFailed);
                                return null;
                            }
                        }
                        else if (adress != null)
                        {
                            choices.Remove("Adress");
                        }
                        break;

                    case "Complete":
                        StopUserCreation("New admin has been created", EventStatus.AccountCreationSuccess);
                        return new Admin(username, password,
                               new PersonInformation(firstName, lastName, dateOfBirth,
                               new ContactInformation(email, phone, adress)));
                    
                    case "Back":
                        StopUserCreation("Admin creation cancelled", EventStatus.AccountCreationFailed);
                        return null;
                }

                if (choices.Count == 2)
                {
                    StopUserCreation("New admin has been created", EventStatus.AccountCreationSuccess);
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
                StopUserCreation("Customer creation cancelled", EventStatus.AccountCreationFailed);
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
                        if (email.EmailAddress == "-1")
                        {
                            choices.Remove("Email");
                            Console.Clear();
                            if (!UserCommunications.AskUserYesOrNo("Continue account creation without email?"))
                            {
                                StopUserCreation("Customer creation cancelled", EventStatus.AccountCreationFailed);
                                return null;
                            }
                        }
                        else if (email != null)
                        {
                            choices.Remove("Email");
                        }
                        break;

                    case "Phone":
                        phone = UserCommunications.GetPhoneFromUser();
                        if (phone.PhoneNumber == "-1")
                        {
                            Console.Clear();
                            if (!UserCommunications.AskUserYesOrNo("Continue account creation without phone number?"))
                            {
                                StopUserCreation("Customer creation cancelled", EventStatus.AccountCreationFailed);
                                return null;
                            }
                        }
                        else if (phone != null)
                        {
                            choices.Remove("Phone");
                        }
                        break;

                    case "Adress":
                        adress = UserCommunications.GetAdressFromUser();
                        if (adress.Country == "-1")
                        {
                            choices.Remove("Adress");
                            Console.Clear();
                            if (!UserCommunications.AskUserYesOrNo("Continue account creation without adress?"))
                            {
                                StopUserCreation("Customer creation cancelled", EventStatus.AccountCreationFailed);
                                return null;
                            }
                        }
                        else if (adress != null)
                        {
                            choices.Remove("Adress");
                        }
                        break;

                    case "Complete":
                        StopUserCreation("New customer has been created", EventStatus.AccountCreationSuccess);
                        return new Customer(username, password,
                               new PersonInformation(firstName, lastName, dateOfBirth,
                               new ContactInformation(email, phone, adress)));
                    
                    case "Back":
                        StopUserCreation("Customer creation cancelled", EventStatus.AccountCreationFailed);
                        return null;
                }

                if (choices.Count == 2)
                {
                    StopUserCreation("New customer has been created", EventStatus.AccountCreationSuccess);
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
