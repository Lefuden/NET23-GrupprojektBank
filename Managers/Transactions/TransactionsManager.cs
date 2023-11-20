using NET23_GrupprojektBank.Managers.Logs;

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

        }
    }
}
