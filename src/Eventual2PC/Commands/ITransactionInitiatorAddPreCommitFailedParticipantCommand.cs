namespace Eventual2PC.Commands
{
    /// <summary>
    /// 事务发起方添加预提交失败的参与方
    /// </summary>
    public interface ITransactionInitiatorAddPreCommitFailedParticipantCommand
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
        /// 事务参与方ID
        /// </summary>
        string ParticipantId { get; }

        /// <summary>
        /// 事务参与方类型
        /// </summary>
        byte ParticipantType { get; }
    }
}