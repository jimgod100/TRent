using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Rent.Models;
namespace Rent.Controllers
{
    public class RentlocationController : Controller
    {
        // GET: Rentlocation
        // GET: RentLocation
        rentEntities db = new rentEntities();
        public ActionResult BIndex(string searchString)
        {
            var brands = db.RentLocation.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                // 使用 Contains 方法進行模糊搜尋，你可以根據實際需求調整查詢條件
                brands = brands.Where(c => c.rentID.Contains(searchString)).ToList();
            }
            return View(brands);
        }

        public ActionResult BCreate()
        {
            return View();
        }

        [HttpPost]

        public ActionResult BCreate(RentLocation brands)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Error = false;
                var temp = db.RentLocation.Where(m => m.rentID == brands.rentID).FirstOrDefault();
                if (temp != null)
                {
                    ViewBag.Error = true;
                    return View(brands);
                }
                db.RentLocation.Add(brands);
                db.SaveChanges();
                return RedirectToAction("BIndex");
            }
            return View(brands);
        }
        public ActionResult BEdit(string rentID)
        {
            var brand = db.RentLocation.Where(m => m.rentID == rentID).FirstOrDefault();
            db.Dispose();
            return View(brand);
        }

        [HttpPost]

        public ActionResult BEdit
            (string rentID, string rentLocation1, string rentAddress)
        {
            var Temp = db.RentLocation.Where(m => m.rentID == rentID).FirstOrDefault();
            Temp.rentID = rentID;
            Temp.rentLocation1 = rentLocation1;
            Temp.rentAddress = rentAddress;
            db.SaveChanges();
            return RedirectToAction("BIndex");
        }
        public ActionResult BDelete(string id)
        {
            var brands = db.RentLocation.Where(m => m.rentID == id).FirstOrDefault();
            db.RentLocation.Remove(brands);
            db.SaveChanges();
            return RedirectToAction("BIndex");
        }
    }
}