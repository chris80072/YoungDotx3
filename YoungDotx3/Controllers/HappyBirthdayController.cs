using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using YoungDotx3.Filters;
using YoungDotx3.Models.Calendar;

namespace YoungDotx3.Controllers
{

    
    public class HappyBirthdayController : Controller
    {
        public ActionResult Index()
        {
            MonthModels month = new MonthModels();
            return View(month);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [AjaxValidateAntiForgeryToken]
        public ActionResult Create(string nickname, string message)
        {
            bool result = false;
            //DingTalkService dingTalkService = new DingTalkService(Session.CurrnetUser());
            //DingTalkSetting setting = dingTalkService.GetDingTalkSetting();

            //if (!string.IsNullOrEmpty(corpId) && !string.IsNullOrEmpty(corpSecret))
            //{
            //    if (corpId.IndexOf("*", StringComparison.Ordinal) < 0)
            //        setting.CorpId = corpId;
            //    if (corpSecret.IndexOf("*", StringComparison.Ordinal) < 0)
            //        setting.CorpSecret = corpSecret;

            //    result = dingTalkService.TestConnection(setting.CorpId, setting.CorpSecret);
            //}

            return Content(JsonConvert.SerializeObject(new { Result = result }), "application/json");
        }
    }
}