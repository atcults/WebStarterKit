using System;
using Core.ReadWrite.Base;

namespace Core.Domain.Model.ValueObjects
{
    public class EmailLog : ValueObject<EmailLog>
    {
        public virtual DateTime TriedOn { get; set; }
        public virtual string Message { get; set; }
    }
}