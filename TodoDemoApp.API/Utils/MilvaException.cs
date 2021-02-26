using System;

namespace TodoDemoApp.API.Utils
{
    public class MilvaException : Exception
    {
        /// <summary>
        /// Exception message
        /// </summary>
        public override string Message { get; }

        public MilvaException(string message) : base(message)
        {
            this.Message = message;
        }
    }
}
