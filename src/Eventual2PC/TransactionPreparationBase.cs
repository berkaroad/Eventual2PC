using System;

namespace Eventual2PC
{
    /// <summary>
    /// 事务准备
    /// </summary>
    [Serializable]
    public abstract class TransactionPreparationBase : ITransactionPreparation
    {
        /// <summary>
        /// 事务准备
        /// </summary>
        protected TransactionPreparationBase() {}

        /// <summary>
        /// 事务准备
        /// </summary>
        /// <param name="participantId">事务参与方ID</param>
        /// <param name="participantType">事务参与方类型</param>
        /// <param name="transactionId">事务ID</param>
        /// <param name="transactionType">事务类型</param>
        /// <param name="initiatorId">事务发起方ID</param>
        /// <param name="initiatorType">事务发起方类型</param>
        protected TransactionPreparationBase(string participantId, byte participantType, string transactionId, byte transactionType, string initiatorId, byte initiatorType)
        {
            ParticipantId = participantId;
            ParticipantType = participantType;
            TransactionId = transactionId;
            TransactionType = transactionType;
            InitiatorId = initiatorId;
            InitiatorType = initiatorType;
        }

        /// <summary>
        /// 事务参与方ID
        /// </summary>
        public string ParticipantId { get; protected set; }

        /// <summary>
        /// 事务参与方类型
        /// </summary>
        public byte ParticipantType { get; protected set; }

        /// <summary>
        /// 事务ID
        /// </summary>
        public string TransactionId { get; protected set; }

        /// <summary>
        /// 事务类型
        /// </summary>
        public byte TransactionType { get; protected set; }

        /// <summary>
        /// 事务发起方ID
        /// </summary>
        public string InitiatorId { get; protected set; }

        /// <summary>
        /// 事务发起方类型
        /// </summary>
        public byte InitiatorType { get; protected set; }

        /// <summary>
        /// 获取准备信息
        /// </summary>
        /// <returns></returns>
        public TransactionPreparationInfo GetTransactionPreparationInfo()
        {
            return new TransactionPreparationInfo(ParticipantId, ParticipantType, TransactionId, TransactionType, InitiatorId, InitiatorType);
        }
    }
}
