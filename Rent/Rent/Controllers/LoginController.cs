using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Rent.Models;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Rent.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        rentEntities db = new rentEntities();
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string Passwd)
        {
            var member = db.Member.Where(m => m.Email == Email && m.Passwd == Passwd).FirstOrDefault();
            if (member == null)
            {
                ViewBag.Message = "帳號或密碼錯誤!!";
                return View();
            }

            if (member.Email == "admin")
            {
                return RedirectToAction("Index", "Car");
            }
            Session["WelCome"] = member.name + "真正高興地見到你!";
            Session["Member"] = member;

            TempData["SelectedCarID"] = ViewBag.SelectedCarID;

            return RedirectToAction("Index", "User");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Front", "User");
        }

        //04-1-1 在HomeController裡分別撰寫Get與Post Register Action
        public ActionResult Register()
        {

            return View();
        }
        //04-1-1 在HomeController裡分別撰寫Get與Post Register Action
        
        //04-1-1 在HomeController裡分別撰寫Get與Post Register Action
        [HttpPost]
        public ActionResult Register(Models.Member Member, HttpPostedFileBase photo)
        {
            string fileName = "";
            if (photo != null)
            {
                if(photo.ContentLength > 0)
                {
                    fileName = Path.GetFileName(photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/Image"), fileName);
                    photo.SaveAs(path);
                }
            }

            Member.licenses = "s";
            Member.bDate = DateTime.Now;

            var member = db.Member.Where(m => m.memberID == Member.memberID).FirstOrDefault();
            if (member == null)
            {

                db.Member.Add(Member);
                db.SaveChanges();

                return RedirectToAction("Login");
            }
            ViewBag.Message = "帳號已有人使用!!";
            return View();

        }

    }
}