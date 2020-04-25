namespace Eventual2PC
{
    /// <summary>
    /// 事务参与方
    /// </summary>
    public interface ITransactionParticipant
    {
        /// <summary>
        /// 预提交（产生预提交成功的领域事件，或抛出领域异常）
        /// </summary>
        /// <param name="transactionPreparation"></param>
        void PreCommit(ITransactionPreparation transactionPreparation);

        /// <summary>
        /// 提交（预提交反馈成功后，才能提交）
        /// </summary>
        /// <param name="transactionId"></param>
        void Commit(string transactionId);

        /// <summary>
        /// 回滚（由事务发起方进行回滚）
        /// </summary>
        /// <param name="transactionId"></param>
        void Rollback(string transactionId);
    }
}
