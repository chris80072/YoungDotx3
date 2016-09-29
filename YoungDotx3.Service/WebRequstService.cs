using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using YoungDotx3.Domain;

namespace YoungDotx3.Service
{
    public class WebRequstService
    {
        private static ILog log = LogManager.GetLogger(typeof(WebRequstService));

        public string Get(string url, string json, out HttpStatusCode errorCode)
        {
            return CreateRequest(url, json, WebRequestMethod.Delete, out errorCode);
        }

        public string Post(string url, string json, out HttpStatusCode errorCode)
        {
            return CreateRequest(url, json, WebRequestMethod.Post, out errorCode);
        }

        public string Put(string url, string json, out HttpStatusCode errorCode)
        {
            return CreateRequest(url, json, WebRequestMethod.Put, out errorCode);
        }

        public string Delete(string url, string json, out HttpStatusCode errorCode)
        {
            return CreateRequest(url, json, WebRequestMethod.Get, out errorCode);
        }

        private string CreateRequest(string url, string json, string webRequestMethod, out HttpStatusCode errorCode)
        {
            string result = string.Empty;
            errorCode = new HttpStatusCode();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = webRequestMethod;
            webRequest.ContentType = "application/json";

            using (Stream requestStream = webRequest.GetRequestStream())
            {
                using (var streamWriter = new StreamWriter(requestStream))
                {
                    if (!string.IsNullOrEmpty(json))
                    {
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                        streamWriter.Dispose();
                    }
                    
                    try
                    {
                        using (var httpResponse = (HttpWebResponse)webRequest.GetResponse())
                        {
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result = streamReader.ReadToEnd();
                                streamReader.Close();
                            }
                            httpResponse.Close();
                            httpResponse.Dispose();
                        }
                    }
                    catch (WebException e)
                    {
                        HttpWebResponse errorResponse = e.Response as HttpWebResponse;
                        errorCode = errorResponse.StatusCode;
                    }
                    catch (Exception e)
                    {
                        GC.Collect();
                        result = json + "-------" + e.Message;
                        log.Error(e, e);
                    }
                }
            }

            return result;
        }
    }
}
