using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace YoungDotx3.Service
{
    public class NetWorkService
    {
        private static ILog log = LogManager.GetLogger(typeof(NetWorkService));

        internal static string GetIpAddress()
        {
            try
            {
                // 取得本機名稱
                string strHostName = Dns.GetHostName();
                // 取得本機的IpHostEntry類別實體，用這個會提示已過時
                //IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

                // 取得本機的IpHostEntry類別實體，MSDN建議新的用法
                IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

                // 取得所有 IP 位址
                foreach (IPAddress ipaddress in iphostentry.AddressList)
                {
                    // 只取得IP V4的Address
                    if (ipaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        //Console.WriteLine("Local IP: " + ipaddress.ToString());
                        return ipaddress.ToString();
                    }
                }

                return string.Empty;
            }
            catch (Exception e)
            {
                log.Error(e, e);
                throw;
            }
        }
    }
}
