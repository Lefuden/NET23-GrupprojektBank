﻿using NET23_GrupprojektBank.Managers.Logs;


namespace NET23_GrupprojektBank.Managers.Transactions
{
    internal class TransactionsManager
    {
        private Queue<Transaction> Transactions { get; set; }
        private List<Log> TransactionLogs { get; set; }
        private Task? _timerTask;
        private readonly PeriodicTimer _timer;
        private readonly CancellationTokenSource _cts = new();
        public TransactionsManager(int timeInMinutes = 15)
        {
            _timer = new(TimeSpan.FromMinutes(timeInMinutes));
            Transactions = new();
            TransactionLogs = new();
        }
        public void Start()
        {
            _timerTask = DoWorkAsync();
        }
        public async Task StopAsync()
        {
            if (_timerTask is null)
            {
                return;
            }
            _cts.Cancel();
            await _timerTask;
            _cts.Dispose();
        }
        private async Task DoWorkAsync()
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    HandleQueuedTransactions();
                }
            }
            catch (OperationCanceledException)
            {
            }
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
            //AnsiConsole.MarkupLine("[green]Taking care of transactions woop woop[/]");

            while (Transactions.Count > 0)
            {
                Transaction transaction = Transactions.Dequeue();

                bool sameCurrencyType = transaction.SourceCurrencyType == transaction.DestinationCurrencyType;


                switch (transaction.TransactionType)
                {
                    case (TransactionType.Withdrawal):
                        if (transaction.Sum > 0)
                        {
                            if (transaction.SourceBankAccount.GetBalance() >= transaction.Sum)
                            {
                                transaction.SourceBankAccount.RemoveBalance(transaction.Sum);
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

                    case (TransactionType.Transfer):
                        if (transaction.Sum > 0)
                        {
                            if (transaction.SourceBankAccount.GetBalance() >= transaction.Sum)
                            {
                                if (sameCurrencyType)
                                {
                                    transaction.SourceBankAccount.RemoveBalance(transaction.Sum);
                                    transaction.DestinationBankAccount.AddBalance(transaction.Sum);
                                    transaction.SourceUser.AddLog(EventStatus.TransferSuccess);
                                    transaction.DestinationUser.AddLog(EventStatus.TransferReceivedSuccess);
                                }
                                else if (!sameCurrencyType)
                                {

                                    decimal convertedSumToTransfer = transaction.SourceBankAccount.ConvertToCurrencyRate(transaction.DestinationCurrencyType, transaction.Sum);
                                    transaction.SourceBankAccount.RemoveBalance(transaction.Sum);
                                    transaction.DestinationBankAccount.AddBalance(convertedSumToTransfer);
                                    transaction.SourceUser.AddLog(EventStatus.TransferSuccess);
                                    transaction.DestinationUser.AddLog(EventStatus.TransferReceivedSuccess);
                                }

                            }
                            else
                            {
                                transaction.SourceUser.AddLog(EventStatus.TransferFailedInsufficientFunds);
                            }
                        }
                        else
                        {
                            transaction.SourceUser.AddLog(EventStatus.TransferFailedNegativeOrZeroSum);
                        }
                        break;

                    case (TransactionType.Loan):
                        if (transaction.Sum > 0)
                        {
                            transaction.SourceBankAccount.AddBalance(transaction.Sum);
                            transaction.SourceBankAccount.AddLoanAndInterest(transaction.Sum, transaction.InterestRate);
                            transaction.SourceUser.AddLog(EventStatus.LoanSuccess);
                        }
                        else
                        {
                            transaction.SourceUser.AddLog(EventStatus.LoanFailedNegativeOrZeroSum);
                        }
                        break;

                    case (TransactionType.Deposit):
                        if (transaction.Sum > 0)
                        {
                            transaction.SourceBankAccount.AddBalance(transaction.Sum);
                            transaction.SourceUser.AddLog(EventStatus.DepositSuccess);
                        }
                        else
                        {
                            transaction.SourceUser.AddLog(EventStatus.DepositFailedNegativeOrZeroSum);
                        }
                        break;

                    default:
                        transaction.SourceUser.AddLog(EventStatus.TransactionFailed);
                        break;
                }
            }

        }
    }
}

