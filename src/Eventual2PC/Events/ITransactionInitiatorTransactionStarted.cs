namespace Eventual2PC.Events
{
    /// <summary>
    /// 事务已开始事件接口
    /// </summary>
    public interface ITransactionInitiatorTransactionStarted<TInitiator>
        where TInitiator : class, ITransactionInitiator
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        string TransactionId { get; }

        /// <summary>
        /// 事务类型
        /// </summary>
        byte TransactionType { get; }
    }
}
