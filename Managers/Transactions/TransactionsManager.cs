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
            while (Transactions.Count > 0)
            {
                Transaction transaction = Transactions.Dequeue();

                // Vilken transaction type är det?

                // Beroende på transaction type så hantera det korrekt

                // är source och destination currency type densamma?

                // Är dem samma så behöver du inte göra en konvertering

                // är dem OLIKA MÅSTE du göra en konvertering av sum till Destination Currency Type

                // Se till att pengar går från rätt konto till rätt konto

                // Se till att dra av pengar från kontot om det behövs

                // Kontrollera att det finns rätt mängd pengar på kontot innan du flyttar pengar

            }
        }
    }
}
