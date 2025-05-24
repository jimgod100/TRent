using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Rent.Models;

namespace Rent.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        rentEntities db = new rentEntities();
        public ActionResult Index(string rentID, string returnID, string carID,string searchString)
        {
            var brands = db.Order.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                // 使用 Contains 方法進行模糊搜尋，你可以根據實際需求調整查詢條件
                brands = brands.Where(c => c.orderID.Contains(searchString)).ToList();
            }
            if (!string.IsNullOrEmpty(rentID))
            {
                if (rentID == "02" || rentID == "03" || rentID == "04" || rentID == "05" || rentID == "06" || rentID == "07" || rentID == "08" || rentID == "09" || rentID == "10" || rentID == "11" || rentID == "12" || rentID == "13" || rentID == "14" || rentID == "15" || rentID == "16" || rentID == "17" || rentID == "18" || rentID == "19" || rentID == "20" || rentID == "21" || rentID == "22" || rentID == "23" || rentID == "24" || rentID == "25" || rentID == "26" || rentID == "27" || rentID == "28")
                {
                    brands = brands.Where(p => p.RentLocation.rentID == rentID).ToList();
                }
            }
            if (!string.IsNullOrEmpty(returnID))
            {
                if (returnID == "02" || returnID == "03" || returnID == "04" || returnID == "05" || returnID == "06" || returnID == "07" || returnID == "08" || returnID == "09" || returnID == "10" || returnID == "11" || returnID == "12" || returnID == "13" || returnID == "14" || returnID == "15" || returnID == "16" || returnID == "17" || returnID == "18" || returnID == "19" || returnID == "20" || returnID == "21" || returnID == "22" || returnID == "23" || returnID == "24" || returnID == "25" || returnID == "26" || returnID == "27" || returnID == "28")
                {
                    brands = brands.Where(p => p.ReturnLocation.returnID == returnID).ToList();
                }
            }

            List<RentLocation> rentLocations = db.RentLocation.ToList();
            ViewBag.RentLocationList = new SelectList(rentLocations, "rentID");
            List<ReturnLocation> returnLocations = db.ReturnLocation.ToList();
            ViewBag.ReturnLocationList = new SelectList(rentLocations, "returnID");

            return View(brands);
        }

        public ActionResult Create(string carID)
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
            ViewBag.SelectedCarID = carID ?? TempData["SelectedCarID"]?.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Create(string rentID, string returnID, DateTime orderDate, string orderTime, DateTime returnDate, string returnTime, string carID, int rentC)
        {
            
            int fUserId = (Session["Member"] as Models.Member).memberID;
            DateTime combinedOrderDateTime = DateTime.Parse(orderDate.ToString("yyyy-MM-dd") + " " + orderTime);
            DateTime combinedReturnDateTime = DateTime.Parse(returnDate.ToString("yyyy-MM-dd") + " " + returnTime);
            //string guid = Guid.NewGuid().ToString();  //產生一組隨機十六進位碼
            //以目前的日期時間加上隨機4碼數字做為訂單編號
            if (combinedOrderDateTime.TimeOfDay == combinedReturnDateTime.TimeOfDay)
            {
                ViewBag.essage = $"時間不可相同";
                return View();
            }
            else { ViewBag.essage = $""; }
            TimeSpan timeDifference = combinedReturnDateTime - combinedOrderDateTime;
            double hours = timeDifference.TotalHours;
            int quantity = (int)Math.Ceiling(hours / 24);
            int price = 1200;
            int amount = price * quantity;
            Random r = new Random();
            string guid = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "").Replace("上午", "").Replace("下午", "") + r.Next(1000, 10000);
            
            
            
            //建立訂單主檔資料
            
            
                Order order = new Order();
                order.orderID = guid;
                order.memberID = fUserId;
                order.rentID = rentID;
                order.returnID = returnID;
                order.orderDate = combinedOrderDateTime;
                order.returnDate = combinedReturnDateTime;
                order.carID = carID;
                order.rentC = rentC;

                db.Order.Add(order);
                db.SaveChanges();

            if (timeDifference.TotalHours < 24)
            {
                // 在24小时内重定向到 "Index"
                return Redirect($"/Static/products.html?orderId={guid}&amount={amount}&name={carID}&quantity={quantity}&price={price}&ordertime={combinedOrderDateTime}&returntime={combinedReturnDateTime}");
            }
            return RedirectToAction("OrderList", "User");

            

            

        }


        public ActionResult Edit(string orderID)
        {
            var brand = db.Order.Where(m => m.orderID == orderID).FirstOrDefault();
            return View(brand);
        }

        [HttpPost]

        public ActionResult Edit(Order brands)
        {
            if (ModelState.IsValid)
            {
                var Temp = db.Order.Where(m => m.orderID == brands.orderID).FirstOrDefault();
                Temp.orderID = brands.orderID;
                Temp.memberID = brands.memberID;
                Temp.rentID = brands.rentID;
                Temp.returnID = brands.returnID;
                Temp.orderDate = brands.orderDate;
                Temp.returnDate = brands.returnDate;
                Temp.carID = brands.carID;

                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(brands);
        }
        public ActionResult Delete(string id)
        {
            var brands = db.Order.Where(m => m.orderID == id).FirstOrDefault();
            db.Order.Remove(brands);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Idex()
        {
            return View();
        }
        public ActionResult RIndex(string rentID, string returnID, string carID, string searchString)
        {
            var brands = db.Order.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                // 使用 Contains 方法進行模糊搜尋，你可以根據實際需求調整查詢條件
                brands = brands.Where(c => c.orderID.Contains(searchString)).ToList();
            }
            if (!string.IsNullOrEmpty(rentID))
            {
                if (rentID == "02" || rentID == "03" || rentID == "04" || rentID == "05" || rentID == "06" || rentID == "07" || rentID == "08" || rentID == "09" || rentID == "10" || rentID == "11" || rentID == "12" || rentID == "13" || rentID == "14" || rentID == "15" || rentID == "16" || rentID == "17" || rentID == "18" || rentID == "19" || rentID == "20" || rentID == "21" || rentID == "22" || rentID == "23" || rentID == "24" || rentID == "25" || rentID == "26" || rentID == "27" || rentID == "28")
                {
                    brands = brands.Where(p => p.RentLocation.rentID == rentID).ToList();
                }
            }
            if (!string.IsNullOrEmpty(returnID))
            {
                if (returnID == "02" || returnID == "03" || returnID == "04" || returnID == "05" || returnID == "06" || returnID == "07" || returnID == "08" || returnID == "09" || returnID == "10" || returnID == "11" || returnID == "12" || returnID == "13" || returnID == "14" || returnID == "15" || returnID == "16" || returnID == "17" || returnID == "18" || returnID == "19" || returnID == "20" || returnID == "21" || returnID == "22" || returnID == "23" || returnID == "24" || returnID == "25" || returnID == "26" || returnID == "27" || returnID == "28")
                {
                    brands = brands.Where(p => p.ReturnLocation.returnID == returnID).ToList();
                }
            }

            List<RentLocation> rentLocations = db.RentLocation.ToList();
            ViewBag.RentLocationList = new SelectList(rentLocations, "rentID");
            List<ReturnLocation> returnLocations = db.ReturnLocation.ToList();
            ViewBag.ReturnLocationList = new SelectList(rentLocations, "returnID");

            return View(brands);
        }
        [HttpPost]
        public JsonResult UpdateRentReturn(string orderId, bool rentReturn)
        {
            var order = db.Order.Find(orderId); // 假設 `db` 是您的資料庫上下文
            if (order != null)
            {
                order.rentReturn = rentReturn;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}