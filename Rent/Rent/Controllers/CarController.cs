using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Rent.Models;



namespace Rent.Controllers
{
    public class CarController : Controller
    {
        rentEntities db = new rentEntities();
        public ActionResult Index(string searchString)
        {
            var cars = db.Car.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                // 使用 Contains 方法進行模糊搜尋，你可以根據實際需求調整查詢條件
                cars = cars.Where(c => c.carID.Contains(searchString)).ToList();
            }
            return View(cars);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(Car cars)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Error = false;
                var temp = db.Car.Where(m => m.carID == cars.carID).FirstOrDefault();
                if (temp != null)
                {
                    ViewBag.Error = true;
                    return View(cars);
                }
                db.Car.Add(cars);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cars);
        }

        public ActionResult Edit(string carID)
        {
            var car = db.Car.Where(m => m.carID == carID).FirstOrDefault();
            return View(car);
        }

        [HttpPost]
        public ActionResult Edit(Car cars)
        {
            if (ModelState.IsValid)
            {
                var Temp = db.Car.Where(m => m.carID == cars.carID).FirstOrDefault();
                Temp.carID = cars.carID;
                Temp.description = cars.description;
                Temp.brandID = cars.brandID;
                Temp.typeID = cars.typeID;
                Temp.seatNume = cars.seatNume;
                Temp.transmission = cars.transmission;
                Temp.luggages = cars.luggages;
                Temp.others = cars.others;
                Temp.Photo = cars.Photo;
                Temp.rent1 = cars.rent1;
                Temp.rent2 = cars.rent2;
                Temp.carDate = cars.carDate;
                Temp.bDate = cars.bDate;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cars);

        }
        public ActionResult Delete(string id)
        {
            var cars = db.Car.Where(m => m.carID == id).FirstOrDefault();
            db.Car.Remove(cars);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}