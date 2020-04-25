namespace Eventual2PC.Events
{
    /// <summary>
    /// 事务参与者已回滚事件
    /// </summary>
    /// <typeparam name="TParticipant"></typeparam>
    /// <typeparam name="TTransactionPreparation"></typeparam>
    public interface ITransactionParticipantRolledback<TParticipant, TTransactionPreparation>
        where TParticipant : class, ITransactionParticipant
        where TTransactionPreparation : class, ITransactionPreparation
    {
        /// <summary>
        /// 事务准备
        /// </summary>
        TTransactionPreparation TransactionPreparation { get; }
    }
}
