using System;
using Core.ReadWrite.Base;

namespace Core.Domain.Model.ValueObjects
{
    public class SmsLog : ValueObject<SmsLog>
    {
        public virtual DateTime TriedOn { get; set; }
        public virtual string Message { get; set; }
    }
}