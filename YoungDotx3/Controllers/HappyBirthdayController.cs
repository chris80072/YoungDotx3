using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Newtonsoft.Json;
using YoungDotx3.Filters;
using YoungDotx3.Models.Calendar;

namespace YoungDotx3.Controllers
{

    
    public class HappyBirthdayController : Controller
    {
        public ActionResult Index()
        {
            Service.HappyBirthdayService service = new Service.HappyBirthdayService();
            var messages = service.GetMessagesByMonth("2016-10");
            MonthModels month = new MonthModels();
            month.Messages.AddRange(messages.Select(val => new MessageModels(val)));
            return View(month);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [AjaxValidateAntiForgeryToken]
        public ActionResult Create(string nickname, string content, string color, string createDate)
        {
            var message = new Domain.Calendar.Message
            {
                Nickname = nickname,
                Content = content,
                Color = color,
                CreateDate = createDate
            };

            Service.HappyBirthdayService service = new Service.HappyBirthdayService();
            bool result = service.CreateMessage(message);

            return Content(JsonConvert.SerializeObject(new { Result = result }), "application/json");
        }
    }
}