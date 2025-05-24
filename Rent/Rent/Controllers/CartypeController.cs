using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Rent.Models;

namespace Rent.Controllers
{
    public class CartypeController : Controller
    {
        // GET: Cartype
        rentEntities db = new rentEntities();
        public ActionResult CIndex(int? searchTypeID)
        {
            var cartype = db.carType.ToList();

            if (searchTypeID.HasValue)
            {
                cartype = cartype.Where(c => c.typeID == searchTypeID.Value).ToList();
            }

            return View(cartype);
        }

        public ActionResult CCreate()
        {
            return View();
        }

        [HttpPost]

        public ActionResult CCreate(carType carTypes)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Error = false;
                var temp = db.carType.Where(m => m.typeID == carTypes.typeID).FirstOrDefault();
                if (temp != null)
                {
                    ViewBag.Error = true;
                    return View(carTypes);
                }
                db.carType.Add(carTypes);
                db.SaveChanges();
                return RedirectToAction("CIndex");
            }
            return View(carTypes);
        }

        public ActionResult CEdit(int id)
        {
            var cartype = db.carType.Where(m => m.typeID == id).FirstOrDefault();
            return View(cartype);
        }

        [HttpPost]

        public ActionResult CEdit(carType carTypes)
        {
            if (ModelState.IsValid)
            {
                var Temp = db.carType.Where(m => m.typeID == carTypes.typeID).FirstOrDefault();
                Temp.typeID = carTypes.typeID;
                Temp.carType1 = carTypes.carType1;

                db.SaveChanges();
                return RedirectToAction("CIndex");

            }
            return View(carTypes);
        }
        public ActionResult CDelete(int id)
        {
            var cartype = db.carType.Where(m => m.typeID == id).FirstOrDefault();
            db.carType.Remove(cartype);
            db.SaveChanges();
            return RedirectToAction("CIndex");
        }
    }
}