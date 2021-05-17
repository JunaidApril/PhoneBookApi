using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.DTO
{
    public interface ISystemResult
    {
        bool IsSuccess { get; }
        string Message { get; }
        string ClientMessage { get; }
    }

    public class SystemResult : ISystemResult
    {
        public string Message { get; private set; }
        public bool IsSuccess { get; private set; }
        public string ClientMessage { get; private set; }

        public SystemResult(string message, string clientMessage = null)
        {
            IsSuccess = false;
            Message = message;
            ClientMessage = clientMessage;
        }

        private SystemResult()
        {
            IsSuccess = true;
        }

        public static SystemResult Success(string message = null, string clientMessage = null)
        {
            return new SystemResult { IsSuccess = true, Message = message, ClientMessage = clientMessage };
        }
    }

    public class SystemResult<T> : ISystemResult
    {
        public string Message { get; private set; }
        public bool IsSuccess { get; private set; }
        public T Data { get; private set; }
        public string ClientMessage { get; private set; }
        /// <summary>
        /// Returns a FAILED result with an accompanying message
        /// </summary>
        /// <param name="message"></param>
        public SystemResult(string message, string clientMessage = null)
        {
            IsSuccess = false;
            Message = message;
            ClientMessage = clientMessage;
        }

        private SystemResult()
        {
            IsSuccess = true;
        }

        /// <summary>
        /// Returns a SUCCESS result with accompanying data payload
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static SystemResult<T> Success(T data, string message = null, string clientMessage = null)
        {
            return new SystemResult<T> { IsSuccess = true, Message = message, Data = data, ClientMessage = clientMessage };
        }

    }
}
