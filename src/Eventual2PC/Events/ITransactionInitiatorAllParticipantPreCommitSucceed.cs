using System.Collections.Generic;

namespace Eventual2PC.Events
{
    /// <summary>
    /// 事务参与方所有预提交已成功事件接口
    /// </summary>
    public interface ITransactionInitiatorAllParticipantPreCommitSucceed<TInitiator>
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
        IEnumerable<TransactionParticipantInfo> TransactionParticipants { get; }
    }
}
