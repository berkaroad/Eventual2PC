namespace Eventual2PC.Events
{
    /// <summary>
    /// 事务参与者预提交失败事件（或领域异常）
    /// </summary>
    /// <typeparam name="TParticipant"></typeparam>
    /// <typeparam name="TTransactionPreparation"></typeparam>
    public interface ITransactionParticipantPreCommitFailed<TParticipant, TTransactionPreparation>
        where TParticipant : class, ITransactionParticipant
        where TTransactionPreparation : class, ITransactionPreparation
    {
        /// <summary>
        /// 事务准备
        /// </summary>
        TTransactionPreparation TransactionPreparation { get; }
    }

    /// <summary>
    /// 事务参与者预提交失败事件（或领域异常）
    /// </summary>
    /// <typeparam name="TParticipant"></typeparam>
    public interface ITransactionParticipantPreCommitFailed<TParticipant>
        where TParticipant : class, ITransactionParticipant
    {
        /// <summary>
        /// 事务准备类型
        /// </summary>
        string TransactionPreparationType { get; }

        /// <summary>
        /// 事务准备
        /// </summary>
        TransactionPreparationInfo TransactionPreparation { get; }
    }
}
