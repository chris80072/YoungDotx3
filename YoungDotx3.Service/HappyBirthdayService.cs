using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YoungDotx3.Domain;
using YoungDotx3.Domain.Calendar;

namespace YoungDotx3.Service
{
    public class HappyBirthdayService
    {
        private static ILog log = LogManager.GetLogger(typeof(HappyBirthdayService));

        public HappyBirthdayService()
        {
            _elasticSearchPath = ConfigurationManager.AppSettings["ElasticSearchPath"];
        }

        private readonly string _elasticSearchPath;

        public List<Message> GetMessagesByMonth(string date)
        {
            string response = string.Empty;
            List<Message> result = new List<Message>();

            try
            {
                var datetime = Convert.ToDateTime(date);
                var firstDayOfMonth = new DateTime(datetime.Year, datetime.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                string json = "{\"query\": {\"range\" : {\"createdate\" : {\"gte\": \"" + firstDayOfMonth.ToString(DateTimeFormat.DateWithHyphen) + "\", \"lte\": \"" + lastDayOfMonth.ToString(DateTimeFormat.DateWithHyphen) + "\"}}}}";

                string url = _elasticSearchPath + "happybirthdaymessage/_search";
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
                        Message message = JsonConvert.DeserializeObject<Message>(aMessageJson.GetValue("_source").ToString());
                        result.Add(message);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                log.Error($"Response = {response}", e);
                throw;
            }
        }

        public bool CreateMessage(Message message)
        {
            string response = string.Empty;

            try
            {
                bool result = false;
                string url = _elasticSearchPath + "happybirthdaymessage";
                string json = message.GetCreateMessageJson();
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
    }
}
