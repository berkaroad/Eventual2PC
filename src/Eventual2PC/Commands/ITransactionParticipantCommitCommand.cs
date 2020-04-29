namespace Eventual2PC.Commands
{
    /// <summary>
    /// 事务参与方提交命令
    /// </summary>
    public interface ITransactionParticipantCommitCommand
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        string TransactionId { get; }
    }
}