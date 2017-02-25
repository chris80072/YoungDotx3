using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using YoungDotx3.DAO.MySQL;

namespace YoungDotx3.Patch
{
    class Program
    {
        static void Main(string[] args)
        {
            MigrateElasticSearchToMariaDB();
        }

        private static void MigrateElasticSearchToMariaDB()
        {
            Console.WriteLine("MigrateElasticSearchToMariaDB  --  Start");

            MySqlConnection connection = Conn();
            connection.Open();

            List<Domain.HappyBirthday.Message> happyBirthdayMessages = GetHappyBirthdayData();
            HappyBirthdayDAO happyBirthdayDao = new HappyBirthdayDAO { Connection = connection };
            happyBirthdayDao.InsertBatch(happyBirthdayMessages);

            List<Domain.Hallelujah.Message> hallelujahMessages = GetHallelujahData();
            HallelujahDAO hallelujahDao = new HallelujahDAO { Connection = connection };
            hallelujahDao.InsertBatch(hallelujahMessages);

            Console.WriteLine("MigrateElasticSearchToMariaDB  --  End");
            Console.ReadLine();
        }

        private static MySqlConnection Conn()
        {
            return new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString);
        }

        private static List<Domain.HappyBirthday.Message> GetHappyBirthdayData()
        {
            List<Domain.HappyBirthday.Message> result = new List<Domain.HappyBirthday.Message>();
            string url = "http://localhost:9200/youngdotx3/happybirthdaymessage/_search";
            HttpStatusCode errorCode;

            WebRequstService webRequstService = new WebRequstService();
            string response = webRequstService.Post(url, "{\"size\" : \"200\"}", out errorCode);

            JObject searchJson = JObject.Parse(response);
            JObject property = searchJson["hits"] as JObject;

            if (property != null)
            {
                JArray hits = property.GetValue("hits") as JArray;
                foreach (var aHit in hits)
                {
                    JObject hit = aHit as JObject;
                    JObject source = hit.GetValue("_source") as JObject;
                    Domain.HappyBirthday.Message message = new Domain.HappyBirthday.Message
                    {
                        Nickname = source.GetValue("nickname").ToString(),
                        Content = source.GetValue("content").ToString(),
                        ColorCode = source.GetValue("color").ToString(),
                        Date = Convert.ToDateTime(source.GetValue("createdate")),
                        IpAddress = source.GetValue("ip").ToString(),
                        IsDelete = false
                    };
                    result.Add(message);
                }
            }

            return result.OrderBy(val => val.CreateDate).ToList();
        }

        private static List<Domain.Hallelujah.Message> GetHallelujahData()
        {
            List<Domain.Hallelujah.Message> result = new List<Domain.Hallelujah.Message>();
            string url = "http://localhost:9200/youngdotx3/messagewall/_search";
            HttpStatusCode errorCode;

            WebRequstService webRequstService = new WebRequstService();
            string response = webRequstService.Post(url, "{\"size\" : \"200\"}", out errorCode);

            JObject searchJson = JObject.Parse(response);
            JObject property = searchJson["hits"] as JObject;

            if (property != null)
            {
                JArray hits = property.GetValue("hits") as JArray;
                foreach (var aHit in hits)
                {
                    JObject hit = aHit as JObject;
                    JObject source = hit.GetValue("_source") as JObject;
                    Domain.Hallelujah.Message message = new Domain.Hallelujah.Message
                    {
                        Nickname = source.GetValue("nickname").ToString(),
                        Content = source.GetValue("content").ToString(),
                        CreateDate = Convert.ToDateTime(source.GetValue("createdatetime")),
                        IpAddress = source.GetValue("ip").ToString(),
                        IsDelete = false
                    };
                    result.Add(message);
                }
            }

            return result.OrderBy(val => val.CreateDate).ToList();
        }
    }
}
