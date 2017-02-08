using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace YoungDotx3.DAO.MySQL
{
    public class TransactionScopeEx
    {
        public static TransactionScope CreateTransactionScope()
        {
            var transactionOptions = new TransactionOptions
            {
#if DEBUG
                IsolationLevel = IsolationLevel.ReadUncommitted

#else
IsolationLevel = IsolationLevel.RepeatableRead
#endif

            };
            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }

        public static TransactionScope CreateTransactionScope(TimeSpan time)
        {
            var transactionOptions = new TransactionOptions
            {
#if DEBUG
                IsolationLevel = IsolationLevel.ReadUncommitted,
                Timeout = time

#else
IsolationLevel = IsolationLevel.RepeatableRead
#endif

            };
            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }
    }
}
