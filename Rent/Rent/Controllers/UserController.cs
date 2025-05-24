using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Rent.Models;

namespace Rent.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        rentEntities db = new rentEntities();
        public ActionResult Index()
        {
            // 检查Session["Member"]是否存在
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

            var cars = db.Car.ToList();
            return View(cars);
        }

        public ActionResult Front()
        {
            return View();
        }

        public ActionResult OrderList()

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
            int fUserId = (Session["Member"] as Models.Member).memberID;
            var orders = db.Order.Where(m => m.memberID == fUserId).OrderByDescending(m => m.orderDate).ToList();


            return View("OrderList", orders);
        }
        public ActionResult MDelete(string id)

        {

            var MEMBER = db.Order.Where(m => m.orderID == id).FirstOrDefault();
            db.Order.Remove(MEMBER);
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }

        public ActionResult MEdit()
        {
            if (Session["Member"] != null)
            {
                // 从Session中获取Member对象
                var memberS = (Models.Member)Session["Member"];

                // 将Member.name的值传递给ViewBag，以便在视图中使用
                ViewBag.WelcomeMessage = $"{memberS.name}";
            }
            else
            {
                // 如果Session["Member"]不存在，可以设置一个默认值或采取其他操作
                ViewBag.WelcomeMessage = "";
            }
            int fUserId = (Session["Member"] as Models.Member).memberID;
            var member = db.Member.Where(m => m.memberID == fUserId).FirstOrDefault();

            return View(member);
        }

        [HttpPost]

        public ActionResult MEdit(Models.Member member)
        {
            if (ModelState.IsValid)
            {
                var Temp = db.Member.Where(m => m.memberID == member.memberID).FirstOrDefault();
                Temp.memberID = member.memberID;
                Temp.name = member.name;
                Temp.Passwd = member.Passwd;
                Temp.ID = member.ID;
                Temp.Gender = member.Gender;
                Temp.Birthday = member.Birthday;
                Temp.licenses = member.licenses;
                Temp.Phone = member.Phone;
                Temp.Email = member.Email;
                Temp.emgyContact = member.emgyContact;
                Temp.relationship = member.relationship;
                Temp.emgyPhone = member.emgyPhone;
                Temp.bDate = member.bDate;
                db.SaveChanges();
                TempData["SuccessMessage"] = "會員資料已成功修改";
                return RedirectToAction("MEdit");

            }
            
            return View(member);

        }

        public ActionResult Edit()
        {
            if (Session["Member"] != null)
            {
                // 从Session中获取Member对象
                var memberS = (Models.Member)Session["Member"];

                // 将Member.name的值传递给ViewBag，以便在视图中使用
                ViewBag.WelcomeMessage = $"{memberS.name}";
            }
            else
            {
                // 如果Session["Member"]不存在，可以设置一个默认值或采取其他操作
                ViewBag.WelcomeMessage = "";
            }
            int fUserId = (Session["Member"] as Models.Member).memberID;
            var member = db.Member.Where(m => m.memberID == fUserId).FirstOrDefault();
            return View(member);
        }

        [HttpPost]

        public ActionResult Edit(Models.Member member, string OldPasswd, string ConfirmPasswd)
        {

            if (ModelState.IsValid)
            {
                var existingMember = db.Member.Where(m => m.memberID == member.memberID).FirstOrDefault();

                // 驗證舊密碼

                if (existingMember.Passwd != OldPasswd)
                {
                    ModelState.AddModelError("OldPasswd", "舊密碼輸入不正確");
                    return View(member);
                }

                if (member.Passwd != ConfirmPasswd)
                {
                    ModelState.AddModelError("ConfirmPasswd", "新密碼和確認密碼不相符。");
                    return View(member);
                }

                // 更新會員資料
                existingMember.name = member.name;
                existingMember.Passwd = member.Passwd;
                existingMember.ID = member.ID;
                existingMember.Gender = member.Gender;
                existingMember.Birthday = member.Birthday;
                existingMember.licenses = member.licenses;
                existingMember.Phone = member.Phone;
                existingMember.Email = member.Email;
                existingMember.emgyContact = member.emgyContact;
                existingMember.relationship = member.relationship;
                existingMember.emgyPhone = member.emgyPhone;
                existingMember.bDate = member.bDate;
                db.SaveChanges();
                TempData["SuccessMessage"] = "會員資料已成功修改";
                return RedirectToAction("Edit");


            }
            return View(member);
        }
        public ActionResult Idex()
        {
            return View();
        }
        // 您的数据库上下文
        public ActionResult Product(string orderId)
        {
            // 从数据库获取订单
            var order = db.Order.FirstOrDefault(o => o.orderID == orderId);

            // 确认订单存在
            if (order == null)
            {
                ViewBag.ErrorMessage = "订单不存在";
                return View("Error"); // 或者返回到一个错误页面
            }

            // 验证订单状态，确保订单未支付且时间上可支付
            if (order.payStat || order.orderDate <= DateTime.Now)
            {
                ViewBag.ErrorMessage = "订单不可支付或已支付";
                return View("Error"); // 或者返回到一个错误页面
            }
            TimeSpan timeDifference = order.returnDate - order.orderDate;
            double hours = timeDifference.TotalHours;
            int quantity = (int)Math.Ceiling(hours / 24);
            int price = 1200;
            int amount = price * quantity;

            // 重定向到支付页面
            return Redirect($"/Static/products.html?orderId={orderId}&amount={amount}&name={order.Car.description}&quantity={quantity}&price={price}&ordertime={order.orderDate}&returntime={order.returnDate}");
        }
        [HttpPost]

        public ActionResult PaymentConfirmation(string orderId)
        {
            var order = db.Order.FirstOrDefault(o => o.orderID == orderId);
            if (order == null)
            {
                return HttpNotFound();
            }

            // 检查订单的支付状态，避免重复处理支付
            if (!order.payStat)
            {
                order.payStat = true; // 标记为已支付
                db.SaveChanges(); // 保存到数据库

                // 可以在这里添加其他逻辑，如发送支付确认邮件等
            }

            return RedirectToAction("OrderList", "User"); // 重定向到订单列表，显示所有订单
        }

    }
}