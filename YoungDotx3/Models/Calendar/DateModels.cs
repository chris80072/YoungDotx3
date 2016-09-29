using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoungDotx3.Models.Calendar
{
    public class DateModels
    {
        public DateModels()
        {
            Messages = new List<MessageModels>();
        }

        public DateModels(Domain.Calendar.Date date)
        {
            Messages = new List<MessageModels>();
            date.Messages.ForEach(val => Messages.Add(new MessageModels(val)));
        }

        public List<MessageModels> Messages { get; set; }
    }
}