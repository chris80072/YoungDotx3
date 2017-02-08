using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungDotx3.Service
{
    public class DBConn
    {
        public static string Conn;
        public static void SetConnString(string conn)
        {
            Conn = conn;
        }
    }
}
