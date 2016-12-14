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
using YoungDotx3.Domain.MessageWall;

namespace YoungDotx3.Service
{
    public class MessageWallService
    {
        private static ILog log = LogManager.GetLogger(typeof(MessageWallService));

        public MessageWallService()
        {
            _elasticSearchPath = ConfigurationManager.AppSettings["ElasticSearchPath"];
        }

        private readonly string _elasticSearchPath;

        public QueryMessages GetMessages(int page)
        {
            string response = string.Empty;
            QueryMessages result = new QueryMessages();

            try
            {
                string json = Domain.MessageWall.Message.GetMessagesJson(page);

                string url = _elasticSearchPath + "messagewall/_search";
                HttpStatusCode errorCode;

                WebRequstService webRequstService = new WebRequstService();
                response = webRequstService.Post(url, json, out errorCode);

                JObject searchJson = JObject.Parse(response);
                JObject property = searchJson["hits"] as JObject;

                if (property != null)
                {
                    JArray hits = property.GetValue("hits") as JArray;
                    result.Total = property.GetValue("total").ToString();
                    
                    foreach (var aHit in hits)
                    {
                        JObject hit = aHit as JObject;
                        JObject source = hit.GetValue("_source") as JObject;
                        
                        Message message = new Message
                        {
                            Id = source.GetValue("id").ToString(),
                            Nickname = source.GetValue("nickname").ToString(),
                            Content = source.GetValue("content").ToString(),
                            CreateDateTime = Convert.ToDateTime(source.GetValue("createdatetime")),
                        };
                        result.Messages.Add(message);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                log.Error($"Response = {response}", e);
                return result;
            }
        }

        public bool CreateMessage(Domain.MessageWall.Message message)
        {
            string response = string.Empty;

            try
            {
                bool result = false;
                string url = _elasticSearchPath + "messagewall";
                string ip = NetWorkService.GetIpAddress();
                long total = GetTotalCount();
                message.Id = (total + 1).ToString();
                string json = message.CreateMessageJson(message.Id, ip);
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

        private long GetTotalCount()
        {
            string response = string.Empty;

            try
            {
                string json = Domain.MessageWall.Message.GetMessagesJson(1);

                string url = _elasticSearchPath + "messagewall/_search";
                HttpStatusCode errorCode;

                WebRequstService webRequstService = new WebRequstService();
                response = webRequstService.Post(url, json, out errorCode);

                JObject searchJson = JObject.Parse(response);
                JObject property = searchJson["hits"] as JObject;

                if (property != null)
                    return Convert.ToInt64(property.GetValue("total"));

                return 0;
            }
            catch (Exception e)
            {
                log.Error($"Response = {response}", e);
                return 0;
            }
        }
    }
}
