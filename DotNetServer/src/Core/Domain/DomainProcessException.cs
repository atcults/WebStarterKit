using System;

namespace Core.Domain
{
    public class DomainProcessException : Exception
    {
        public DomainProcessException(string message) : base(message)
        {
        }
    }
}