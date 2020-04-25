namespace Eventual2PC.Events
{
    /// <summary>
    /// 预提交失败的事务参与方已添加事件接口
    /// </summary>
    public interface ITransactionInitiatorPreCommitFailedParticipantAdded<TInitiator>
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
        /// 事务参与方信息
        /// </summary>
        TransactionParticipantInfo TransactionParticipant { get; }
    }
}
