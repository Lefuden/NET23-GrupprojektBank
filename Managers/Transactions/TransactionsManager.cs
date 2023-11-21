using NET23_GrupprojektBank.BankAccounts;
using NET23_GrupprojektBank.Currency;
﻿using NET23_GrupprojektBank.Managers.Logs;


namespace NET23_GrupprojektBank.Managers.Transactions
{
    internal class TransactionsManager
    {
        private Queue<Transaction> Transactions { get; set; }
        private List<Log> TransactionLogs { get; set; }
        public TransactionsManager(Queue<Transaction>? transactions = null)
        {
            if (transactions is not null)
            {
                Transactions = transactions;
            }
            else
            {
                Transactions = new();
            }
            TransactionLogs = new();
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction is not null)
            {
                Transactions.Enqueue(transaction);
                TransactionLogs.Add(new Log(DateTime.UtcNow, "Transaction Manager Added Transaction To Queue Successfully"));
            }
            else
            {
                TransactionLogs.Add(new Log(DateTime.UtcNow, "Transaction Manager Failed To Add Transaction To Queue"));
            }
        }
        private void HandleQueuedTransactions()
        {
            bool sameCurrencyType = (transaction.SourceCurrencyType == transaction.DestinationCurrencyType);
            double amountToConvert;
            Dictionary<CurrencyType, double> amountToConvert = new Dictionary<CurrencyType, double>;

            while (Transactions.Count > 0)
            {
                Transaction transaction = Transactions.Dequeue();
               
                switch (transaction.TransactionType)
                {
                    case (TransactionType.Withdrawal):
                    if (transaction.Sum > 0)
                    {
                        if (transaction.SourceBankAccount.GetBalance() >= transaction.Sum)
                        {
                            transaction.SourceBankAccount.Remove(transaction.Sum);
                            transaction.SourceUser.AddLog(EventStatus.WithdrawalSuccess);
                        }
                    }
                    else
                    {
                        transaction.SourceUser.AddLog(EventStatus.WithdrawalFailedInsufficientFunds);
                    }
                    break;

                    case (TransactionType.Transfer):
                        if (transaction.Sum > 0)
                        {
                            if (transaction.SourceBankAccount.GetBalance() >= transaction.Sum)
                            {
                                if (sameCurrencyType)
                                {
                                    transaction.SourceBankAccount.Remove(transaction.Sum);
                                    transaction.DestinationBankAccount.Add(transaction.Sum);
                                    transaction.SourceUser.AddLog(EventStatus.TransferSuccess);
                                    transaction.DestinationUser.AddLog(EventStatus.TransferReceivedSuccess);
                                }
                                else if (sameCurrencyType != true)
                                {
                                    transaction.SourceBankAccount.ConvertToCurrencyRate(amountToConvert);
                                    transaction.SourceBankAccount.Remove(transaction.Sum);
                                    transaction.DestinationBankAccount.Add(transaction.Sum);
                                    transaction.SourceUser.AddLog(EventStatus.TransferSuccess);
                                    transaction.DestinationUser.AddLog(EventStatus.TransferReceivedSuccess);
                                }
                            }
                            else
                            {
                                transaction.SourceUser.AddLog(EventStatus.TransferFailedInsufficientFunds);
                            }
                        }
                        break;
                            
                    case (TransactionType.Loan):
                        if (transaction.Sum > 0)
                        {
                            transaction.SourceBankAccount.AddLog(transaction.Sum);
                            transaction.SourceUser.AddLog(EventStatus.LoanSuccess);
                        }
                        else
                        {
                            transaction.SourceUser.AddLoag(EventStatus.LoanFailedNegativeOrZeroSum)
                        }
                        break;
                            
                    case (TransactionType.Deposit):
                        if (transaction.Sum > 0)
                        {
                            transaction.SourceBankAccount.Add();
                            transaction.SourceUser.AddLog(EventStatus.DepositSuccess);
                        }
                        else
                        {
                            transaction.SourceUser.AddLog(EventStatus.DepositFailedNegativeOrZeroSum);
                        }
                        break;
                }
            }
            else
            {
                transaction.SourceUser.AddLog(EventStatus.TransactionFailed);
            }
        }
    }
}

