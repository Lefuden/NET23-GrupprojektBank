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
                return EventStatus.TransactionManagerAddedToQueueSuccess;
            }
            else
            {
                return EventStatus.TransactionManagerAddedToQueueFailed;
            }
        }

        private void HandleQueuedTransactions()
        {

        }
    }
}
