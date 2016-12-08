using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using YoungDotx3.Domain;
using YoungDotx3.Filters;
using YoungDotx3.Models.MessageWall;

namespace YoungDotx3.Controllers
{
    public class MessageWallController : Controller
    {
        // GET: MessageWall
        public ActionResult Index()
        {
            //Service.MessageWallService service = new Service.MessageWallService();
            //var messages = service.GetMessages();
            //List<MessageModels> messageModelses = new List<MessageModels>();
            //messageModelses.AddRange(messages.Select(val => new MessageModels(val)));
            //return View(messageModelses);
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [AjaxValidateAntiForgeryToken]
        public ActionResult GetMessages(int page)
        {
            Service.MessageWallService service = new Service.MessageWallService();
            var messages = service.GetMessages(page);
            List<MessageModels> messageModelses = new List<MessageModels>();
            messageModelses.AddRange(messages.Messages.Select(val => new MessageModels(val)));

            return Content(JsonConvert.SerializeObject(new { messageModels = messageModelses }), "application/json");
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [AjaxValidateAntiForgeryToken]
        public ActionResult Create(string nickname, string content)
        {
            var message = new Domain.MessageWall.Message
            {
                Nickname = nickname,
                Content = content,
                CreateDateTime = DateTime.Now
            };

            Service.MessageWallService service = new Service.MessageWallService();
            bool result = service.CreateMessage(message);

            return Content(JsonConvert.SerializeObject(new { isSuccess = result, messageModel = new MessageModels(message) }), "application/json");
        }
    }
}