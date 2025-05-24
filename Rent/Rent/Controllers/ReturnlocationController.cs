using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rent.Models;
namespace Rent.Controllers
{
    public class ReturnlocationController : Controller
    {
        // GET: Returnlocation
        rentEntities db = new rentEntities();
        public ActionResult BIndex(string searchString)
        {
            var brands = db.ReturnLocation.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                // 使用 Contains 方法進行模糊搜尋，你可以根據實際需求調整查詢條件
                brands = brands.Where(c => c.returnID.Contains(searchString)).ToList();
            }
            return View(brands);
        }
        public ActionResult BCreate()
        {
            return View();
        }

        [HttpPost]

        public ActionResult BCreate(ReturnLocation brands)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Error = false;
                var temp = db.ReturnLocation.Where(m => m.returnID == brands.returnID).FirstOrDefault();
                if (temp != null)
                {
                    ViewBag.Error = true;
                    return View(brands);
                }
                db.ReturnLocation.Add(brands);
                db.SaveChanges();
                return RedirectToAction("BIndex");
            }
            return View(brands);
        }
        public ActionResult BEdit(string brandID)
        {
            var brand = db.ReturnLocation.Where(m => m.returnID == brandID).FirstOrDefault();
            return View(brand);
        }

        [HttpPost]

        public ActionResult BEdit(ReturnLocation brands)
        {
            if (ModelState.IsValid)
            {
                var Temp = db.ReturnLocation.Where(m => m.returnID == brands.returnID).FirstOrDefault();
                Temp.returnID = brands.returnID;
                Temp.returnLocation1 = brands.returnLocation1;
                Temp.returnAddress = brands.returnAddress;
                db.SaveChanges();
                return RedirectToAction("BIndex");

            }
            return View(brands);
        }
        public ActionResult BDelete(string id)
        {
            var brands = db.ReturnLocation.Where(m => m.returnID == id).FirstOrDefault();
            db.ReturnLocation.Remove(brands);
            db.SaveChanges();
            return RedirectToAction("BIndex");
        }
    }
}