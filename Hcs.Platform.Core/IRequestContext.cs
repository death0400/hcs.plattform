namespace Hcs.Platform.Core
{
    public interface IRequestContext
    {
        System.Transactions.TransactionScope TransactionScope { get; }
    }
}