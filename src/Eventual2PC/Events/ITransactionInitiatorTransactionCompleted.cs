namespace Eventual2PC.Events
{
    /// <summary>
    /// 事务已完成事件接口
    /// </summary>
    public interface ITransactionInitiatorTransactionCompleted<TInitiator>
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

        /// <summary>
        /// 事务是否成功
        /// </summary>
        bool IsCommitSuccess { get; }
    }
}
