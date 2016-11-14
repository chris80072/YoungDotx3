using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YoungDotx3.Domain;

namespace YoungDotx3.Service
{
    public class MessageWallService
    {
        private static ILog log = LogManager.GetLogger(typeof(HappyBirthdayService));

        public MessageWallService()
        {
            _elasticSearchPath = ConfigurationManager.AppSettings["ElasticSearchPath"];
        }

        private readonly string _elasticSearchPath;

        public List<Domain.MessageWall.Message> GetMessages()
        {
            string response = string.Empty;
            List<Domain.MessageWall.Message> result = new List<Domain.MessageWall.Message>();

            try
            {
                string json = "{\"sort\": { \"createdatetime\": { \"order\": \"desc\" } }}";

                string url = _elasticSearchPath + "messagewall/_search";
                HttpStatusCode errorCode;

                WebRequstService webRequstService = new WebRequstService();
                response = webRequstService.Post(url, json, out errorCode);

                JObject searchJson = JObject.Parse(response);
                JObject property = searchJson["hits"] as JObject;
                //result.Total = Int32.Parse(property.GetValue("total").ToString());
                if (property != null)
                {
                    JArray hits = property.GetValue("hits") as JArray;
                    foreach (var aHit in hits)
                    {
                        JObject aMessageJson = aHit as JObject;
                        Domain.MessageWall.Message message = JsonConvert.DeserializeObject<Domain.MessageWall.Message>(aMessageJson.GetValue("_source").ToString());
                        result.Add(message);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                log.Error($"Response = {response}", e);
                return new List<Domain.MessageWall.Message>();
            }
        }

        public bool CreateMessage(Domain.MessageWall.Message message)
        {
            string response = string.Empty;

            try
            {
                bool result = false;
                string url = _elasticSearchPath + "messagewall";
                string ip = GetIpAddress();
                string json = message.GetCreateMessageJson(ip);
                HttpStatusCode errorCode;

                WebRequstService webRequstService = new WebRequstService();
                response = webRequstService.Post(url, json, out errorCode);
                dynamic responseObj = JsonConvert.DeserializeObject(response);

                result = Convert.ToBoolean(responseObj["created"]);
                return result;
            }
            catch (Exception e)
            {
                log.Error($"Response = {response}", e);
                throw;
            }
        }

        internal string GetIpAddress()
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
