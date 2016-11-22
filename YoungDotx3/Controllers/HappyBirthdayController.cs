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
            MonthModels month = new MonthModels();
            month.Today = DateTime.Now.ToString("yyyy-MM-dd");
            var messages = service.GetMessagesByMonth();
            month.Messages.AddRange(messages.Select(val => new MessageModels(val)));
            return View(month);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [AjaxValidateAntiForgeryToken]
        public ActionResult Create(string nickname, string content, string color, string createDate)
        {
            bool result = false;

            if (Convert.ToDateTime(createDate) > DateTime.Now)
            {
                var message = new Domain.Calendar.Message
                {
                    Nickname = nickname,
                    Content = content,
                    Color = color,
                    CreateDate = createDate
                };

                Service.HappyBirthdayService service = new Service.HappyBirthdayService();
                result = service.CreateMessage(message);
            }

            return Content(JsonConvert.SerializeObject(new { Result = result }), "application/json");
        }
    }
}