namespace Eventual2PC.Commands
{
    /// <summary>
    /// 事务参与方回滚命令
    /// </summary>
    public interface ITransactionParticipantRollbackCommand
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        string TransactionId { get; }
    }
}