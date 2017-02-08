using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using YoungDotx3.Domain;

namespace YoungDotx3.Models.Calendar
{
    public class MessageModels
    {
        public MessageModels(Domain.Calendar.Message message)
        {
            this.id = message.Id;
            this.title = message.Nickname;
            this.content = message.Content;
            this.start = message.CreateDate;
            this.color = message.Color;
        }

        public MessageModels(Domain.HappyBirthday.Message message)
        {
            this.id = message.Id;
            this.title = message.Nickname;
            this.content = message.Content;
            this.start = message.Date.ToString(DateTimeFormat.DateWithHyphen);
            this.color = message.ColorCode;
        }

        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string start { get; set; }
        public string color { get; set; }
    }
}