using NET23_GrupprojektBank.Currency;

namespace NET23_GrupprojektBank.Managers.Transactions
{
    internal class TransactionsManager
    {
        private Queue<Transaction> Transactions { get; set; }
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
        }

        public EventStatus AddTransaction(Transaction transaction)
        {
            if (transaction is not null)
            {
                Transactions.Enqueue(transaction);
                HandleQueuedTransactions();
                return EventStatus.TransactionManagerAddedToQueueSuccess;
            }
            else
            {
                return EventStatus.TransactionManagerAddedToQueueFailed;
            }
        }

        private void HandleQueuedTransactions()
        {
            while (Transactions.Count > 0)
            {
                Transaction transaction = Transactions.Dequeue();
                Dictionary<CurrencyType, double> exchangeRate = new();
                bool isDifferentCurrencyTypes = transaction.SourceCurrencyType != transaction.DestinationCurrencyType;
                decimal sum;

                // är source och destination currency type densamma?
                if (isDifferentCurrencyTypes)
                {
                    exchangeRate = CurrencyExchangeRate.GetCurrentCurrencyExchangeRate(transaction.DestinationCurrencyType);
                    // är dem OLIKA MÅSTE du göra en konvertering av sum till Destination Currency Type
                    sum = (decimal)exchangeRate[transaction.DestinationCurrencyType] * transaction.Sum;
                }
                else
                {
                    // Är dem samma så behöver du inte göra en konvertering
                    sum = transaction.Sum;
                }

                // Vilken transaction type är det?
                // Beroende på transaction type så hantera det korrekt
                switch (transaction.TransactionType)
                {
                    case TransactionType.Withdrawal:
                        if (sum > 0)
                        {
                            if (transaction.SourceBankAccount.GetBalance() >= sum)
                            {
                                // Remove money from source bank account
                                transaction.SourceBankAccount.Remove(sum);
                                transaction.SourceUser.AddLog(EventStatus.WithdrawalSuccess);
                            }
                            else
                            {
                                transaction.SourceUser.AddLog(EventStatus.WithdrawalFailedInsufficientFunds);
                            }
                        }
                        else
                        {
                            transaction.SourceUser.AddLog(EventStatus.WithdrawalFailedNegativeOrZeroSum);
                        }
                        break;

                    case TransactionType.Deposit:
                        if (sum > 0)
                        {
                            // Add money to source bank account
                            transaction.SourceBankAccount.Add(sum);
                            transaction.SourceUser.AddLog(EventStatus.DepositSuccess);
                        }
                        else
                        {
                            transaction.SourceUser.AddLog(EventStatus.DepositFailedNegativeOrZeroSum);
                        }
                        break;

                    case TransactionType.Loan:
                        if (sum > 0)
                        {
                            // Add money to source bank account
                            transaction.SourceBankAccount.Add(sum);
                            transaction.SourceUser.AddLog(EventStatus.LoanSuccess);
                        }
                        else
                        {
                            transaction.SourceUser.AddLog(EventStatus.LoanFailedNegativeOrZeroSum);
                        }
                        break;

                    case TransactionType.Transfer:
                        if (sum > 0 && transaction.Sum > 0)
                        {
                            if (transaction.SourceBankAccount.GetBalance() >= transaction.Sum)
                            {
                                // Remove money from source bank account 
                                if (isDifferentCurrencyTypes)
                                {
                                    transaction.SourceBankAccount.Remove(transaction.Sum);
                                    transaction.DestinationBankAccount.Add(sum);
                                }
                                else
                                {
                                    transaction.SourceBankAccount.Remove(sum);
                                    transaction.DestinationBankAccount.Add(sum);
                                }

                                transaction.SourceUser.AddLog(EventStatus.TransferSuccess);
                                if (transaction.SourceUser != transaction.DestinationUser)
                                {
                                    transaction.DestinationUser.AddLog(EventStatus.TransferReceivedSuccess);
                                }
                            }
                            else
                            {
                                transaction.SourceUser.AddLog(EventStatus.TransferFailedInsufficientFunds);
                                //if (transaction.SourceUser != transaction.DestinationUser)
                                //{
                                //    transaction.DestinationUser.AddLog(EventStatus.TransferFailedInsufficientFunds);
                                //}
                            }
                        }
                        else
                        {
                            transaction.SourceUser.AddLog(EventStatus.TransferFailedNegativeOrZeroSum);
                            //if (transaction.SourceUser != transaction.DestinationUser)
                            //{ 
                            //    transaction.DestinationUser.AddLog(EventStatus.TransferFailedNegativeOrZeroSum);
                            //}
                        }
                        break;

                }
            }
        }
    }
}
