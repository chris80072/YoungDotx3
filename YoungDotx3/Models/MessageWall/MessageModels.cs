using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoungDotx3.Domain.Hallelujah;

namespace YoungDotx3.Models.MessageWall
{
    public class MessageModels
    {
        public MessageModels(Message message)
        {
            this.Id = message.Id;
            this.Nickname = message.Nickname;
            this.Content = message.Content;
            this.CreateDate = message.CreateDate.ToString(Domain.DateTimeFormat.DateWithHyphen);
            this.CreateTime = message.CreateDate.ToString(Domain.DateTimeFormat.Time);
        }

        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public int Sequence { get; set; }
    }
}