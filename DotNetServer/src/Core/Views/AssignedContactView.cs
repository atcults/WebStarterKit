using System;
using Common.Enumerations;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;
using Newtonsoft.Json;

namespace Core.Views
{
    [ViewName("AssignedContactView")]
    public class AssignedContactView : AuditedView
    {
        public Guid ContactId { get; set; }
        public string ContactName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public Gender Gender { get; set; }
        public string GenderValue
        {
            get { return Gender == null ? "" : Gender.Value; }
            set { Gender = Gender.FromValue(value); }
        }

        public string GenderName
        {
            get { return Gender == null ? "" : Gender.DisplayName; }
            set { Gender = Gender.FromDisplay(value); }
        }

        [JsonIgnore]
        public ContactType ContactType { get; set; }
        public string ContactTypeValue
        {
            get { return ContactType == null ? "" : ContactType.Value; }
            set { ContactType = ContactType.FromValue(value); }
        }
        public string ContactTypeName
        {
            get { return ContactType == null ? "" : ContactType.DisplayName; }
            set { ContactType = ContactType.FromDisplay(value); }
        }

        [JsonIgnore]
        public Language PrimaryLanguage { get; set; }
        public string PrimaryLanguageValue
        {
            get { return PrimaryLanguage == null ? "" : PrimaryLanguage.Value; }
            set { PrimaryLanguage = Language.FromValue(value); }
        }

        public string PrimaryLanguageName
        {
            get { return PrimaryLanguage == null ? "" : PrimaryLanguage.DisplayName; }
            set { PrimaryLanguage = Language.FromDisplay(value); }
        }

        [JsonIgnore]
        public Language SecondaryLanguage { get; set; }
        public string SecondaryLanguageValue
        {
            get { return SecondaryLanguage == null ? "" : SecondaryLanguage.Value; }
            set { SecondaryLanguage = Language.FromValue(value); }
        }

        public string SecondaryLanguageName
        {
            get { return SecondaryLanguage == null ? "" : SecondaryLanguage.DisplayName; }
            set { SecondaryLanguage = Language.FromDisplay(value); }
        }

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