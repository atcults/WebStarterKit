using System;
using Common.Enumerations;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;
using Newtonsoft.Json;

namespace Core.Views
{
    [ViewName("HealthView")]
    public class HealthView : IView
    {
        [JsonIgnore]
        public HealthType HealthType { get; set; }

        public string HealthTypeValue
        {
            get { return HealthType == null ? "" : HealthType.Value; }
            set { HealthType = HealthType.FromValue(value); }
        }

        public string HealthTypeName
        {
            get { return HealthType == null ? "" : HealthType.DisplayName; }
            set { HealthType = HealthType.FromDisplay(value); }
        }

        public string Value { get; set; }
        public DateTime? RecordTime { get; set; }
        public Guid Id { get; set; }
    }
}