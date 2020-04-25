namespace Eventual2PC
{
    /// <summary>
    /// 事务准备
    /// </summary>
    public interface ITransactionPreparation
    {
        /// <summary>
        /// 事务参与方ID
        /// </summary>
        string ParticipantId { get; }

        /// <summary>
        /// 事务参与方类型
        /// </summary>
        byte ParticipantType { get; }

        /// <summary>
        /// 事务ID
        /// </summary>
        string TransactionId { get; }

        /// <summary>
        /// 事务类型
        /// </summary>
        byte TransactionType { get; }

        /// <summary>
        /// 事务发起方ID
        /// </summary>
        string InitiatorId { get; }

        /// <summary>
        /// 事务发起方类型
        /// </summary>
        byte InitiatorType { get; }

        /// <summary>
        /// 获取准备信息
        /// </summary>
        /// <returns></returns>
        TransactionPreparationInfo GetTransactionPreparationInfo();
    }
}
