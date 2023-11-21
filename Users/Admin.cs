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
            var (username, password, firstName, lastName, dateOfBirth) = UserCommunications.GetBasicsFromUser(existingUsernames);
            if (username == "-1") return null;

            if (!UserCommunications.AskUserYesOrNo("Add more information?"))
            {
                AnsiConsole.MarkupLine("[green]Admin has been created[/]");
                AddLog(EventStatus.AccountCreationSuccess);
                return new Admin(username, password,
                       new PersonInformation(firstName, lastName, dateOfBirth,
                       new ContactInformation(new Email(""))));
            }

            Email email = UserCommunications.GetEmailFromUser();
            if (email.EmailAddress == "-1") return null;

            if (!UserCommunications.AskUserYesOrNo("Add more information?"))
            {
                AnsiConsole.MarkupLine("[green]Admin has been created[/]");
                AddLog(EventStatus.AccountCreationSuccess);
                return new Admin(username, password,
                       new PersonInformation(firstName, lastName, dateOfBirth,
                       new ContactInformation(email)));
            }

            Phone phone = UserCommunications.GetPhoneFromUser();
            if (phone.PhoneNumber == "-1") return null;

            if (!UserCommunications.AskUserYesOrNo("Add more information?"))
            {
                AnsiConsole.MarkupLine("[green]Admin has been created[/]");
                AddLog(EventStatus.AccountCreationSuccess);
                return new Admin(username, password,
                       new PersonInformation(firstName, lastName, dateOfBirth,
                       new ContactInformation(email, phone)));
            }

            Address adress = UserCommunications.GetAdressFromUser();
            if (adress.Country == "-1") return null;
            AnsiConsole.MarkupLine("[green]Admin has been created[/]");
            AddLog(EventStatus.AccountCreationSuccess);
            return new Admin(username, password,
                   new PersonInformation(firstName, lastName, dateOfBirth,
                   new ContactInformation(email, phone, adress)));
        }

        public Customer CreateCustomerAccount(List<string> existingUsernames)
        {
            var (username, password, firstName, lastName, dateOfBirth) = UserCommunications.GetBasicsFromUser(existingUsernames);

            if (username == "-1")
            {
                return null;
            }

            if (!UserCommunications.AskUserYesOrNo("Add more information?"))
            {
                AnsiConsole.MarkupLine("[green]Customer has been created[/]");
                AddLog(EventStatus.AccountCreationSuccess);
                return new Customer(username, password,
                       new PersonInformation(firstName, lastName, dateOfBirth,
                       new ContactInformation(new Email(""))));
            }

            Email email = UserCommunications.GetEmailFromUser();

            if (email.EmailAddress == "-1")
            {
                return null;
            }

            if (!UserCommunications.AskUserYesOrNo("Add more information?"))
            {
                AnsiConsole.MarkupLine("[green]Customer has been created[/]");
                AddLog(EventStatus.AccountCreationSuccess);
                return new Customer(username, password,
                       new PersonInformation(firstName, lastName, dateOfBirth,
                       new ContactInformation(email)));
            }

            Phone phone = UserCommunications.GetPhoneFromUser();

            if (phone.PhoneNumber == "-1")
            {
                return null;
            }

            if (!UserCommunications.AskUserYesOrNo("Add more information?"))
            {
                AnsiConsole.MarkupLine("[green]Customer has been created[/]");
                AddLog(EventStatus.AccountCreationSuccess);
                return new Customer(username, password,
                       new PersonInformation(firstName, lastName, dateOfBirth,
                       new ContactInformation(email, phone)));
            }

            Address adress = UserCommunications.GetAdressFromUser();

            if (adress.Country == "-1")
            {
                return null;
            }

            AnsiConsole.MarkupLine("[green]Customer has been created[/]");
            AddLog(EventStatus.AccountCreationSuccess);
            return new Customer(username, password,
                new PersonInformation(firstName, lastName, dateOfBirth,
                new ContactInformation(email, phone, adress)));
        }





        public void UpdateCurrencyExchangeRate()
        {
            EventStatus taskUpdateStatus = CurrencyExchangeRate.UpdateCurrencyExchangeRateAsync(UserType).Result;
            AddLog(taskUpdateStatus);
        }
    }
}
