using System;

namespace Common.SystemSettings
{
    [Serializable]
    public class DatabaseConfig
    {
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public bool IntegratedSecurity { get; set; }

        public string GetConnectionString()
        {
            return IntegratedSecurity ?
                       string.Format("Server={0};DataBase={1};Integrated Security=True;", Server, DatabaseName) :
                       string.Format("Server={0};DataBase={3};User Id={1};Password={2};", Server, UserName, Password, DatabaseName);
        }

        public string GetMasterConnectionString()
        {
            return IntegratedSecurity ?
                       string.Format("Server={0};DataBase=master;Integrated Security=True;", Server) :
                       string.Format("Server={0};DataBase=master;User Id={1};Password={2};", Server, UserName, Password);
        }
    }
}
