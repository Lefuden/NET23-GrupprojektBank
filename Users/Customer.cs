﻿using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Managers;
using NET23_GrupprojektBank.Managers.Logic;
using NET23_GrupprojektBank.Users.UserInformation;

namespace NET23_GrupprojektBank.Users
{
    internal class Customer : User
    {
        protected List<BankAccount> BankAccounts { get; set; }

        public Customer(string userName, string password, PersonInformation person) : base(userName, password, person)
        {
            UserType = UserType.Customer;
            BankAccounts = new List<BankAccount>();
        }

        public void ViewBankAccount()
        {
            if (BankAccounts.Count != 0)
            {
                foreach (var account in BankAccounts)
                {
                    //amazing interface
                    Console.WriteLine(account);
                    Console.ReadKey();
                    //back to menu
                }
            }
            else
            {
                //amazing interface
                Console.WriteLine($"no accounts found\nwould you like to open a new account?\n1. yes\n2. no");
                var input = int.TryParse(Console.ReadLine(), out int choice);
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("enter account name, account type: checking or savings (1,2), balance, currencytype (0-50)");
                        Console.ReadLine();
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
                //add new? y/n -> call method CreateBankAccount() or back to menu
            }
        }

        public UserChoice CreateBankAccount()
        {
            return UserChoice.CreateChecking;
            return UserChoice.CreateSavings;
            return UserChoice.Back;

            //wanna make checking or savings or go back?

            //BankAccount.cs
            //string BankAccountNumber, BankAccountName
            //Enum BankAccountType, CurrencyType
        }

        public EventStatus CreateCheckingAccount()
        {
            //logic to create account goes here
        }

        public EventStatus CreateSavingsAccount()
        {
            //logic to create account goes here
        }

        private void CreateTransaction() //: Transaction input parameters for various transactions/loans/withdrawals
        {
            //take input from various Make-methods to then create the transaction of corresponding type
        }

        public void MakeLoan()
        {
            //BankAccount.GetBalance();
            //logic to create a loan
            //send to createtransaction for completion
        }

        public void MakeWithdrawal()
        {
            //BankAccount.GetBalance();
            //logic to create a withdrawal
            //send to createtransaction for completion
        }

        public void MakeDeposit()
        {
            //BankAccount.GetBalance();
            //logic to create a deposit
            //send to createtransaction for completion
        }

        public void MakeTransfer()
        {
            //BankAccount.GetBalance();
            //logic to create a transfer
            //send to createtransaction for completion
        }
    }
}
