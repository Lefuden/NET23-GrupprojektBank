using NET23_GrupprojektBank.Currency;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Managers.UserInteraction;
using NET23_GrupprojektBank.Users.UserContactInformation;
using NET23_GrupprojektBank.Users.UserInformation;
using NET23_GrupprojektBank.Users.UserInformation.UserContactInformation.Specifics;

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
            Email email = new("", "");
            Phone phone = new("");
            Address address = new("", "", "", "");

            var userInfo = UserCommunications.GetBasicsFromUser(existingUsernames);
            if (userInfo.Username == "-1")
            {
                UserCommunications.StopUserCreation("Admin creation cancelled");
                AddLog(EventStatus.AdminAccountCreationFailed);
                return null;
            }
            var remainingChoices = UserCommunications.GetAdminCreateUserChoiceList();

            while (true)
            {
                var choice = UserCommunications.AdminCreateUserMenu(remainingChoices);
                switch (choice)
                {
                    case UserChoice.Email:
                        remainingChoices = UserCommunications.UpdateAdminUserChoiceList(remainingChoices, choice);
                        var emailInfo = UserCommunications.GetEmailFromUser();
                        if (emailInfo.DoesUserWantToContinue is not true) return null;
                        email = emailInfo.Email;
                        break;

                    case UserChoice.Phone:
                        remainingChoices = UserCommunications.UpdateAdminUserChoiceList(remainingChoices, choice);
                        var phoneInfo = UserCommunications.GetPhoneFromUser();
                        if (phoneInfo.DoesUserWantToContinue is not true) return null;
                        phone = phoneInfo.Phone;
                        break;

                    case UserChoice.Address:
                        remainingChoices = UserCommunications.UpdateAdminUserChoiceList(remainingChoices, choice);
                        var adressInfo = UserCommunications.GetAdressFromUser();
                        if (adressInfo.DoesUserWantToContinue is not true) return null;
                        address = adressInfo.Address;
                        break;

                    case UserChoice.Complete:
                        UserCommunications.StopUserCreation("New admin has been created");
                        AddLog(EventStatus.AdminAccountCreationSuccess);
                        return new Admin(userInfo.Username, userInfo.Password,
                               new PersonInformation(userInfo.FirstName, userInfo.LastName, userInfo.DateOfBirth,
                               new ContactInformation(email, phone, address)));

                    case UserChoice.Back:
                        UserCommunications.StopUserCreation("Admin creation cancelled");
                        AddLog(EventStatus.AdminAccountCreationFailed);
                        return null;
                }

                if (remainingChoices.Count == 2)
                {
                    UserCommunications.StopUserCreation("New admin has been created");
                    AddLog(EventStatus.AdminAccountCreationSuccess);
                    return new Admin(userInfo.Username, userInfo.Password,
                               new PersonInformation(userInfo.FirstName, userInfo.LastName, userInfo.DateOfBirth,
                               new ContactInformation(email, phone, address)));
                }
            }
        }

        public Customer CreateCustomerAccount(List<string> existingUsernames)
        {
            Email email = new("", "");
            Phone phone = new("");
            Address address = new("", "", "", "");

            var userInfo = UserCommunications.GetBasicsFromUser(existingUsernames);
            if (userInfo.Username == "-1")
            {
                UserCommunications.StopUserCreation("Customer creation cancelled");
                AddLog(EventStatus.AdminAccountCreationFailed);
                return null;
            }
            var remainingChoices = UserCommunications.GetAdminCreateUserChoiceList();

            while (true)
            {
                var choice = UserCommunications.AdminCreateUserMenu(remainingChoices);
                switch (choice)
                {
                    case UserChoice.Email:
                        remainingChoices = UserCommunications.UpdateAdminUserChoiceList(remainingChoices, choice);
                        var emailInfo = UserCommunications.GetEmailFromUser();
                        if (emailInfo.DoesUserWantToContinue is not true) return null;
                        email = emailInfo.Email;
                        break;

                    case UserChoice.Phone:
                        remainingChoices = UserCommunications.UpdateAdminUserChoiceList(remainingChoices, choice);
                        var phoneInfo = UserCommunications.GetPhoneFromUser();
                        if (phoneInfo.DoesUserWantToContinue is not true) return null;
                        phone = phoneInfo.Phone;
                        break;

                    case UserChoice.Address:
                        remainingChoices = UserCommunications.UpdateAdminUserChoiceList(remainingChoices, choice);
                        var adressInfo = UserCommunications.GetAdressFromUser();
                        if (adressInfo.DoesUserWantToContinue is not true) return null;
                        address = adressInfo.Address;
                        break;

                    case UserChoice.Complete:
                        UserCommunications.StopUserCreation("New admin has been created");
                        AddLog(EventStatus.AdminAccountCreationSuccess);
                        return new Admin(userInfo.Username, userInfo.Password,
                               new PersonInformation(userInfo.FirstName, userInfo.LastName, userInfo.DateOfBirth,
                               new ContactInformation(email, phone, address)));

                    case UserChoice.Back:
                        UserCommunications.StopUserCreation("Admin creation cancelled");
                        AddLog(EventStatus.AdminAccountCreationFailed);
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
        private void AddLogCreateUserAccountEvents(UserType userType, EventStatus customerEventStatus, EventStatus adminEventStatus)
        {
            switch (userType)
            {
                case UserType.Customer:
                    AddLog(customerEventStatus);
                    break;
                case UserType.Admin:
                    AddLog(adminEventStatus);
                    break;
            }
        }
        public User CreateUserAccount(List<string> existingUsernames, UserType userType)
        {
            Email email = new("", "");
            Phone phone = new("");
            Address address = new("", "", "", "");
            UserChoice choice;
            EventStatus eventStatus;
            (UserChoice UserChoice, EventStatus EventStatus) proceedInfo;
            var userInfo = UserCommunications.GetBasicsFromUser(existingUsernames);
            if (userInfo.Username == "-1")
            {
                UserCommunications.StopUserCreation($"{userType.ToString()} creation cancelled");
                AddLog(EventStatus.AdminAccountCreationFailed);
                return null;
            }
            var remainingChoices = UserCommunications.GetAdminCreateUserChoiceList();

            while (true)
            {
                choice = UserCommunications.AdminCreateUserMenu(remainingChoices);
                switch (choice)
                {
                    case UserChoice.Email:
                        remainingChoices = UserCommunications.UpdateAdminUserChoiceList(remainingChoices, choice);
                        var emailInfo = UserCommunications.GetEmailFromUser();
                        proceedInfo = UserCommunications.CancelOrProceedUserCreation(choice, userType, emailInfo.DoesUserWantToContinue);
                        choice = proceedInfo.UserChoice;
                        eventStatus = proceedInfo.EventStatus;
                        email = emailInfo.Email;
                        break;

                    case UserChoice.Phone:
                        remainingChoices = UserCommunications.UpdateAdminUserChoiceList(remainingChoices, choice);
                        var phoneInfo = UserCommunications.GetPhoneFromUser();
                        proceedInfo = UserCommunications.CancelOrProceedUserCreation(choice, userType, phoneInfo.DoesUserWantToContinue);
                        choice = proceedInfo.UserChoice;
                        eventStatus = proceedInfo.EventStatus;
                        phone = phoneInfo.Phone;
                        break;

                    case UserChoice.Address:
                        remainingChoices = UserCommunications.UpdateAdminUserChoiceList(remainingChoices, choice);
                        var adressInfo = UserCommunications.GetAdressFromUser();
                        proceedInfo = UserCommunications.CancelOrProceedUserCreation(choice, userType, adressInfo.DoesUserWantToContinue);
                        choice = proceedInfo.UserChoice;
                        eventStatus = proceedInfo.EventStatus;
                        address = adressInfo.Address;
                        break;

                    default:
                        break;
                }
                if (remainingChoices.Count == 2)
                {
                    choice = UserChoice.Complete;
                    break;
                }

                if (choice == UserChoice.Back)
                {
                    break;
                }
            }
            if (choice is not UserChoice.Complete)
            {
                UserCommunications.StopUserCreation($"{userType.ToString()} creation cancelled");
                AddLogCreateUserAccountEvents(userType, Event)
                    AddLog(EventStatus.AdminAccountCreationFailed);
            }

            UserCommunications.StopUserCreation($"New {userType.ToString()} has been created");
            AddLog(EventStatus.AdminAccountCreationSuccess);
            return new Admin(userInfo.Username, userInfo.Password,
                   new PersonInformation(userInfo.FirstName, userInfo.LastName, userInfo.DateOfBirth,
                   new ContactInformation(email, phone, address)));
        }
        public void UpdateCurrencyExchangeRate()
        {
            EventStatus taskUpdateStatus = CurrencyExchangeRate.UpdateCurrencyExchangeRateAsync(UserType).Result;
            AddLog(taskUpdateStatus);
        }
    }
}
