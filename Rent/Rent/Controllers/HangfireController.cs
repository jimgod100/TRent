using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rent.Controllers
{
    public class HangfireController : Controller
    {
        // GET: Hangfire
        public ActionResult Index()
        {
            // 直接返回 Hangfire 儀表板視圖
            return View();
        }
    }
}