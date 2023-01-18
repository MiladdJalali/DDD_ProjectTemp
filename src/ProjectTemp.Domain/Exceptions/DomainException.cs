using System.Runtime.Serialization;

namespace ProjectTemp.Domain.Exceptions
{
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, string detail)
            : base(message)
        {
            Detail = detail;
        }

        protected DomainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string? Detail { get; }
    }
}
