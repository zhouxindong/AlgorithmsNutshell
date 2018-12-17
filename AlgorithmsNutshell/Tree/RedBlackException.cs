using System;
using System.Runtime.Serialization;

namespace AlgorithmsNutshell.Tree
{
    [Serializable]
    public class RedBlackException : Exception
    {
        public RedBlackException()
        { }

        public RedBlackException(string message)
            : base(message)
        { }

        public RedBlackException(string message, Exception exception)
            : base(message, exception)
        { }

        protected RedBlackException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }
    }
}