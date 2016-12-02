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
        int total { get; set; }

        public QueryMessages()
        {
            Messages = new List<Message>();
            total = 0;
        }
    }
}
