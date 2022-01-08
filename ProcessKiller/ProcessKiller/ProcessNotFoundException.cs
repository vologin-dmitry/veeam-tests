using System;

namespace ProcessKiller
{
    [Serializable]
    public class ProcessNotFoundException : ArgumentException
    {
        public ProcessNotFoundException()
        {
        }

        public ProcessNotFoundException(string message) : base(message)
        {
        }

        public ProcessNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ProcessNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
