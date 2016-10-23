using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoungDotx3.Models.Calendar
{
    public class MessageModels
    {
        public MessageModels(Domain.Calendar.Message message)
        {
            this.title = message.Content;
            this.start = message.CreateDate;
            this.color = message.Color;
        }

        public string title { get; set; }
        public string start { get; set; }
        public string color { get; set; }
    }
}