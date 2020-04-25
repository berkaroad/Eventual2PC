using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventual2PC
{
    /// <summary>
    /// 事务参与方信息
    /// </summary>
    [Serializable]
    public sealed class TransactionParticipantInfo
    {
        /// <summary>
        /// 事务准备
        /// </summary>
        /// <param name="participantId">事务参与方ID</param>
        /// <param name="participantType">事务参与方类型</param>
        public TransactionParticipantInfo(string participantId, byte participantType)
        {
            ParticipantId = participantId;
            ParticipantType = participantType;
        }

        /// <summary>
        /// 事务参与方ID
        /// </summary>
        public string ParticipantId { get; private set; }

        /// <summary>
        /// 事务参与方类型
        /// </summary>
        public byte ParticipantType { get; private set; }

        /// <summary>
        /// 参与者是否已存在
        /// </summary>
        /// <param name="participants">参与者信息列表</param>
        /// <returns></returns>
        public bool IsParticipantAlreadyExists(IEnumerable<TransactionParticipantInfo> participants)
        {
            return participants != null && participants.Any(a => a.ParticipantId == ParticipantId);
        }

        /// <summary>
        /// 验证必须不存在
        /// </summary>
        /// <param name="participants">参与者信息列表</param>
        public void ValidateParticipantMustNotExists(IEnumerable<TransactionParticipantInfo> participants)
        {
            if (IsParticipantAlreadyExists(participants))
            {
                throw new ApplicationException($"{ParticipantId} has already exists.");
            }
        }
    }
}
