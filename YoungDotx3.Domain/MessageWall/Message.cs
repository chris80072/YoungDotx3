﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YoungDotx3.Domain.MessageWall
{
    public class Message
    {
        public string Id { get; set; } = string.Empty;

        private string _nickname = string.Empty;
        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value.Length > 20 ? value.Substring(0, 20) : value; }
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return _content; }
            set { _content = value.Length > 500 ? value.Substring(0, 500) : value; }
        }
        public string CreateDateTime { get; set; } = string.Empty;

        public string GetCreateMessageJson(string ip)
        {
            StringWriter sw = new StringWriter();
            JsonTextWriter jsonText = new JsonTextWriter(sw);
            jsonText.WriteStartObject();
            jsonText.WritePropertyName("nickname");
            jsonText.WriteValue(Nickname);
            jsonText.WritePropertyName("content");
            jsonText.WriteValue(Content);
            jsonText.WritePropertyName("createdatetime");
            jsonText.WriteValue(CreateDateTime);
            jsonText.WritePropertyName("ip");
            jsonText.WriteValue(ip);
            jsonText.WriteEndObject();

            return sw.ToString(); ;
        }
    }
}