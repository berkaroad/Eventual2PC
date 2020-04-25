using System.Collections.Generic;

namespace Eventual2PC.Events
{
    /// <summary>
    /// 事务参与方任意一个预提交已失败的事件接口
    /// </summary>
    public interface ITransactionInitiatorAnyParticipantPreCommitFailed<TInitiator>
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
        /// 成功预提交的事务参与方信息
        /// </summary>
        IEnumerable<TransactionParticipantInfo> PreCommitSucceedTransactionParticipants { get; }
    }
}
