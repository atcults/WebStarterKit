using System;
using System.Data;
using System.Data.SqlClient;
using Common.Enumerations;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
    [EntityName("Contact")]
    public class Contact : AuditedEntity
    {
        public Gender Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public ContactType ContactType { get; set; }
        public Language PrimaryLanguage { get; set; }
        public Language SecondaryLanguage { get; set; }
        
        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);
            Gender = Gender.FromValue(dataReader.ReadNullSafeString("GenderValue")); 
            Mobile = dataReader.ReadNullSafeString("Mobile");
            Email = dataReader.ReadNullSafeString("Email");
            ContactType = ContactType.FromValue(dataReader.ReadNullSafeString("ContactTypeValue"));
            PrimaryLanguage = Language.FromValue(dataReader.ReadNullSafeString("PrimaryLanguageValue"));
            SecondaryLanguage = Language.FromValue(dataReader.ReadNullSafeString("SecondaryLanguageValue"));
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);
            cmd.Parameters.Add("@GenderValue", SqlDbType.VarChar).Value = BaseEnumeration.GetDbNullSafe(Gender); 
            cmd.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = (object)Mobile ?? DBNull.Value;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = (object)Email ?? DBNull.Value;
            cmd.Parameters.Add("@ContactTypeValue", SqlDbType.VarChar).Value = BaseEnumeration.GetDbNullSafe(ContactType);
            cmd.Parameters.Add("@PrimaryLanguageValue", SqlDbType.VarChar).Value = BaseEnumeration.GetDbNullSafe(PrimaryLanguage);
            cmd.Parameters.Add("@SecondaryLanguageValue", SqlDbType.VarChar).Value = BaseEnumeration.GetDbNullSafe(SecondaryLanguage);
        }
    }
}