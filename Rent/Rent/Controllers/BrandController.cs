using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Rent.Models;

namespace Rent.Controllers
{
    public class BrandController : Controller
    {
        // GET: Brand
        rentEntities db = new rentEntities();
        public ActionResult BIndex(string searchString)
        {
            var brands = db.Brand.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                // 使用 Contains 方法進行模糊搜尋，你可以根據實際需求調整查詢條件
                brands = brands.Where(c => c.brandID.Contains(searchString)).ToList();
            }
            return View(brands);
        }

        public ActionResult BCreate()
        {
            return View();
        }

        [HttpPost]

        public ActionResult BCreate(Brand brands)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Error = false;
                var temp = db.Brand.Where(m => m.brandID == brands.brandID).FirstOrDefault();
                if (temp != null)
                {
                    ViewBag.Error = true;
                    return View(brands);
                }
                db.Brand.Add(brands);
                db.SaveChanges();
                return RedirectToAction("BIndex");
            }
            return View(brands);
        }

        public ActionResult BEdit(string brandID)
        {
            var brand = db.Brand.Where(m => m.brandID == brandID).FirstOrDefault();
            return View(brand);
        }

        [HttpPost]

        public ActionResult BEdit(Brand brands)
        {
            if (ModelState.IsValid)
            {
                var Temp = db.Brand.Where(m => m.brandID == brands.brandID).FirstOrDefault();
                Temp.brandID = brands.brandID;
                Temp.English = brands.English;
                Temp.Chinese = brands.Chinese;
                db.SaveChanges();
                return RedirectToAction("BIndex");

            }
            return View(brands);
        }
        public ActionResult BDelete(string id)
        {
            var brands = db.Brand.Where(m => m.brandID == id).FirstOrDefault();
            db.Brand.Remove(brands);
            db.SaveChanges();
            return RedirectToAction("BIndex");
        }
    }
}