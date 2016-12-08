using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungDotx3.Domain.MessageWall
{
    public class QueryMessages
    {
        public List<Message> Messages { get; set; }
        public string Total { get; set; }
        
        public QueryMessages()
        {
            Messages = new List<Message>();
            Total = string.Empty;
        }
    }
}
