using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungDotx3.Domain.Calendar
{
    public class Date
    {
        public Date()
        {
            Messages = new List<Message>();
        }

        public List<Message> Messages { get; set; }
    }
}
