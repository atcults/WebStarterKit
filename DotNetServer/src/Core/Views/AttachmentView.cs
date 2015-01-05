using System;
using Common.Enumerations;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;
using Newtonsoft.Json;

namespace Core.Views
{
    [ViewName("AttachmentView")]
    public class AttachmentView : AuditedView
    {
        public string Tags { get; set; }
        public string FileType { get; set; }
        public decimal? FileSize { get; set; }
        public string FileHashCode { get; set; }
        public byte[] FileData { get; set; }

        [JsonIgnore]
        public EntityType EntityType { get; set; }

        public string EntityTypeValue
        {
            get { return EntityType == null ? "" : EntityType.Value; }
            set { EntityType = EntityType.FromValue(value); }
        }

        public string EntityTypeName
        {
            get { return EntityType == null ? "" : EntityType.DisplayName; }
            set { EntityType = EntityType.FromDisplay(value); }
        }

        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }
    }
}