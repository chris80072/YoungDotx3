using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using YoungDotx3.Domain;
using YoungDotx3.Filters;
using YoungDotx3.Models.MessageWall;
using YoungDotx3.Service;
using YoungDotx3.Domain.Hallelujah;

namespace YoungDotx3.Controllers
{
    public class HallelujahController : Controller
    {
        // GET: Hallelujah
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
        public ActionResult GetMessages(string id)
        {
            HallelujahService service = new HallelujahService();
            var messages = service.GetMessages(id);
            List<MessageModels> messageModelses = new List<MessageModels>();
            messageModelses.AddRange(messages.Select(val => new MessageModels(val)));
            return Content(JsonConvert.SerializeObject(new { messages = messageModelses }), "application/json");
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [AjaxValidateAntiForgeryToken]
        public ActionResult Create(string nickname, string content)
        {
            bool result = false;
            Message message = null;
            try
            {
                message = new Message
                {
                    Nickname = nickname,
                    Content = content,
                    CreateDate = DateTime.Now
                };

                HallelujahService service = new HallelujahService();
                service.CreateMessage(message);

                result = !string.IsNullOrEmpty(message.Id);
            }
            catch (Exception)
            {
            }

            return Content(JsonConvert.SerializeObject(new { isSuccess = result, messageModel = new MessageModels(message) }), "application/json");
        }
    }
}