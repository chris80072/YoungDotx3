using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoungDotx3.Models.Calendar
{
    public class MonthModels
    {
        public MonthModels()
        {
            Messages = new List<MessageModels>();
        }

        public MonthModels(Domain.Calendar.Month month)
        {
            Messages = new List<MessageModels>();
            month.Messages.ForEach(val => Messages.Add(new MessageModels(val)));
            Today = month.Today.ToString(Domain.DateTimeFormat.DateWithHyphen);
        }

        public List<MessageModels> Messages { get; set; }
        public string Today { get; set; }
    }
}