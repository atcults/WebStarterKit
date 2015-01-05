using System.Data;
using System.Data.SqlClient;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
    [EntityName("Template")]
    public class Template : Entity
    {
        public string Name { get; set; }
        public string MailBody { get; set; }
        public string SmsBody { get; set; } 

        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);
            Name = dataReader.ReadNullSafeString("Name");
            MailBody = dataReader.ReadNullSafeString("MailBody");
            SmsBody = dataReader.ReadNullSafeString("SmsBody");
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
            cmd.Parameters.Add("@MailBody", SqlDbType.NVarChar).Value = MailBody;
            cmd.Parameters.Add("@SmsBody", SqlDbType.NVarChar).Value = SmsBody;
        }
    }
}