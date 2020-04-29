namespace Eventual2PC.Commands
{
    /// <summary>
    /// 事务参与方预提交命令
    /// </summary>
    public interface ITransactionParticipantPreCommitCommand
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
        /// 事务发起方ID
        /// </summary>
        string InitiatorId { get; }
        
        /// <summary>
        /// 事务发起方类型
        /// </summary>
        byte InitiatorType { get; }
    }
}