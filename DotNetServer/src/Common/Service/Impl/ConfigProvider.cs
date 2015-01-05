using System;
using System.Data;
using System.Data.SqlClient;
using Common.SerializerHelper;
using Common.SystemSettings;

namespace Common.Service.Impl
{
    public class ConfigProvider
    {
        protected ConfigProvider() { }

        private static EmailConfig _emailConfig;

        public static EmailConfig GetEmailConfig()
        {
            if (_emailConfig != null)
            {
                if (_emailConfig.MailHost != null && !string.IsNullOrWhiteSpace(_emailConfig.MailHost))
                    return _emailConfig;
            }

            _emailConfig = GetConfiguration<EmailConfig>();
            return _emailConfig;
        }

        public static void SetEmailConfig(EmailConfig emailConfig, bool isPersist)
        {
            if (isPersist)
            {
                SaveConfiguration(emailConfig);
            }

            _emailConfig = emailConfig;
        }

        private static SmsConfig _smsConfig;

        public static SmsConfig GetSmsConfig()
        {
            if (_smsConfig != null)
            {
                if (_smsConfig.ServiceUrl != null && !string.IsNullOrWhiteSpace(_smsConfig.ServiceUrl))
                    return _smsConfig;
            }

            _smsConfig = GetConfiguration<SmsConfig>();

            return _smsConfig;
        }

        public static void SetSmsConfig(SmsConfig smsConfig, bool isPersist)
        {
            if (isPersist)
            {
                SaveConfiguration(smsConfig);
            }

            _smsConfig = smsConfig;
        }

        private static NetworkConfig _networkConfig;

        public static NetworkConfig GetNetworkConfig()
        {
            if (_networkConfig != null)
            {
                return _networkConfig;
            }

            _networkConfig = GetConfiguration<NetworkConfig>();

            return _networkConfig;
        }

        public static void SetNetworkConfig(NetworkConfig networkConfig, bool isPersist)
        {
            if (isPersist)
            {
                SaveConfiguration(networkConfig);
            }

            _networkConfig = networkConfig;
        }

        protected static T GetConfiguration<T>()
        {
            if (!GetGeneralConfig().StoreConfigInDatabase)
            {
                return Xml2Obj<T>.Load();
            }

            var configName = typeof(T).Name;
            using (var connection = new SqlConnection(GetDatabaseConfig().GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    using (var dbCommand = connection.CreateCommand())
                    {
                        dbCommand.CommandText = "SELECT Value from CoreConfiguration where ConfigName = @ConfigName";
                        dbCommand.Parameters.Add("@ConfigName", SqlDbType.VarChar).Value = configName;
                        var serialization = new CustomXmlSerializer();
                        var value = dbCommand.ExecuteScalar();
                        if (value != null) return serialization.Deserialize<T>(value.ToString());
                    }
                    return default(T);
                }
                catch (Exception)
                {
                    return default(T);
                }
                finally
                {
                    connection.Dispose();
                }
            }
        }

        protected static void SaveConfiguration<T>(T config)
        {
            if (!GetGeneralConfig().StoreConfigInDatabase)
            {
                Xml2Obj<T>.Save(config);
                return;
            }

            var configName = typeof(T).Name;

            var isNew = GetConfiguration<T>() == null;

            var serialization = new CustomXmlSerializer();
            var newValue = serialization.Serialize(config);
            using (var connection = new SqlConnection(GetDatabaseConfig().GetConnectionString()))
            {
                connection.Open();
                using (var dbCommand = connection.CreateCommand())
                {
                    dbCommand.CommandText = isNew ? "INSERT INTO [dbo].[CoreConfiguration] ([ConfigName], [Value]) SELECT @ConfigName, @Value" :
                                                 "UPDATE [dbo].[CoreConfiguration] SET [Value] = @Value WHERE ConfigName = @ConfigName";
                    dbCommand.Parameters.Add("@ConfigName", SqlDbType.VarChar).Value = configName;
                    dbCommand.Parameters.Add("@Value", SqlDbType.VarChar).Value = newValue;

                    dbCommand.ExecuteNonQuery();
                }
            }
        }

        #region GeneralAndDbConfig

        private static DatabaseConfig _databaseConfig;

        public static DatabaseConfig GetDatabaseConfig()
        {
            if (_databaseConfig != null)
            {
                if (_databaseConfig.Server != null && !string.IsNullOrWhiteSpace(_databaseConfig.Server))
                return _databaseConfig;
            }

            _databaseConfig = GetConfiguration<DatabaseConfig>();

            return _databaseConfig;
        }

        public static void SetDatabaseConfig(DatabaseConfig databaseConfig, bool isPersist)
        {
            if (isPersist)
            {
                Xml2Obj<DatabaseConfig>.Save(databaseConfig);
            }

            _databaseConfig = databaseConfig;
        }

        private static GeneralConfig _generalConfig;

        public static GeneralConfig GetGeneralConfig()
        {
            return _generalConfig ?? (_generalConfig = Xml2Obj<GeneralConfig>.Load());
        }

        public static void SetGeneralConfig(GeneralConfig generalConfig, bool isPersist)
        {
            if (isPersist)
            {
                Xml2Obj<GeneralConfig>.Save(generalConfig);
            }

            _generalConfig = generalConfig;
        }

        #endregion
    }
}
