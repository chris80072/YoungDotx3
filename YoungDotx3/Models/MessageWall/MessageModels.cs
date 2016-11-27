using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoungDotx3.Models.MessageWall
{
    public class MessageModels
    {
        public MessageModels(Domain.MessageWall.Message message)
        {
            this.Id = message.Id;
            this.Nickname = message.Nickname;
            this.Content = message.Content;
            this.CreateDate = message.CreateDateTime.ToString(Domain.DateTimeFormat.DateWithHyphen);
            this.CreateTime = message.CreateDateTime.ToString(Domain.DateTimeFormat.Time);
        }

        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
    }
}