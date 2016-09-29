using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoungDotx3.Domain.Calendar
{
    public class Month
    {
        public Month()
        {
            Messages = new List<Message>();
            Today = DateTime.Today;
        }

        public List<Message> Messages { get; set; }
        public DateTime Today { get; set; }
    }
}
