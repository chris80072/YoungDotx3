using System.Configuration;
using YoungDotx3.Service;

namespace YoungDotx3.App_Start
{
    public class DBConfig
    {
        public static void Register()
        {
            DBConn.SetConnString(ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString);
        }
    }
}