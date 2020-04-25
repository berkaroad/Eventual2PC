using System;

namespace Eventual2PC
{
    /// <summary>
    /// 未知的TransactionPreparation异常
    /// </summary>
    [Serializable]
    public sealed class UnknownTransactionPreparationException : ApplicationException
    {
        /// <summary>
        /// 未知的TransactionPreparation异常
        /// </summary>
        public UnknownTransactionPreparationException() : base() { }

        /// <summary>
        /// 未知的TransactionPreparation异常
        /// </summary>
        /// <param name="message"></param>
        public UnknownTransactionPreparationException(string message) : base(message) { }

        /// <summary>
        /// 未知的TransactionPreparation异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnknownTransactionPreparationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
