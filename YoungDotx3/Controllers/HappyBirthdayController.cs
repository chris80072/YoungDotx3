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
            //month.Today = DateTime.Now.ToString("yyyy-MM-dd");
            //var messages = service.GetMessagesByMonth();
            var messages = service.GetMessages();
            month.Messages.AddRange(messages.Select(val => new MessageModels(val)));
            return View(month);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [AjaxValidateAntiForgeryToken]
        public ActionResult Create(string nickname, string content, string color, string createDate)
        {
            bool result = false;

            try
            {
                if (Convert.ToDateTime(createDate) > DateTime.Now)
                {
                    var message = new Domain.HappyBirthday.Message
                    {
                        Nickname = nickname,
                        Content = content,
                        ColorCode = color,
                        Date = Convert.ToDateTime(createDate)
                    };

                    Service.HappyBirthdayService service = new Service.HappyBirthdayService();
                    service.CreateMessage(message);
                    result = !string.IsNullOrEmpty(message.Id);
                }
            }
            catch (Exception)
            {
            }

            return Content(JsonConvert.SerializeObject(new { Result = result }), "application/json");
        }
    }
}