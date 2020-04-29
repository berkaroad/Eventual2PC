namespace Eventual2PC
{
    /// <summary>
    /// 事务发起方
    /// </summary>
    public interface ITransactionInitiator
    {
        /// <summary>
        /// 添加预提交成功的参与者信息
        /// </summary>
        /// <param name="transactionId">事务ID</param>
        /// <param name="transactionType">事务类型</param>
        /// <param name="participantInfo">事务参与方信息</param>
        void AddPreCommitSuccessParticipant(string transactionId, byte transactionType, TransactionParticipantInfo participantInfo);

        /// <summary>
        /// 添加预提交失败的参与者信息
        /// </summary>
        /// <param name="transactionId">事务ID</param>
        /// <param name="transactionType">事务类型</param>
        /// <param name="participantInfo">事务参与方信息</param>
        void AddPreCommitFailedParticipant(string transactionId, byte transactionType, TransactionParticipantInfo participantInfo);

        /// <summary>
        /// 添加已提交的参与者信息
        /// </summary>
        /// <param name="transactionId">事务ID</param>
        /// <param name="transactionType">事务类型</param>
        /// <param name="participantInfo">事务参与方信息</param>
        void AddCommittedParticipant(string transactionId, byte transactionType, TransactionParticipantInfo participantInfo);

        /// <summary>
        /// 添加已回滚的参与者信息
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionType">事务类型</param>
        /// <param name="participantInfo">事务参与方信息</param>
        void AddRolledbackParticipant(string transactionId, byte transactionType, TransactionParticipantInfo participantInfo);

        /// <summary>
        /// 是否事务在处理中
        /// </summary>
        bool IsTransactionProcessing { get; }

        /// <summary>
        /// 当前事务ID
        /// </summary>
        string CurrentTransactionId { get; }

        /// <summary>
        /// 当前事务类型
        /// </summary>
        byte CurrentTransactionType { get; }
    }
}
