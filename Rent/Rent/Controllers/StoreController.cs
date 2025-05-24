using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rent.Models;
namespace Rent.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        rentEntities db = new rentEntities();
        public ActionResult SIndex()
        {
            if (Session["Member"] != null)
            {
                // 从Session中获取Member对象
                var member = (Models.Member)Session["Member"];

                // 将Member.name的值传递给ViewBag，以便在视图中使用
                ViewBag.WelcomeMessage = $"{member.name}";
            }
            else
            {
                // 如果Session["Member"]不存在，可以设置一个默认值或采取其他操作
                ViewBag.WelcomeMessage = "";
            }
            var brands = db.RentLocation.OrderBy(m => m.rentID).ToList();
            return View(brands);
        }
        public ActionResult TIndex()
        {
            if (Session["Member"] != null)
            {
                // 从Session中获取Member对象
                var member = (Models.Member)Session["Member"];

                // 将Member.name的值传递给ViewBag，以便在视图中使用
                ViewBag.WelcomeMessage = $"{member.name}";
            }
            else
            {
                // 如果Session["Member"]不存在，可以设置一个默认值或采取其他操作
                ViewBag.WelcomeMessage = "";
            }
            var brands = db.ReturnLocation.OrderBy(m => m.returnID).ToList();
            return View(brands);
        }
    }
}