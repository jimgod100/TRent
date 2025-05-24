using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Ajax.Utilities;
using Rent.Models;

namespace Rent.Controllers
{
    public class MemberController : Controller
    {
        // GET: member
        rentEntities db = new rentEntities();
        public ActionResult MIndex(string searchString, int page = 1)
        {
            var member = db.Member.ToList();
            memberList data = new memberList();
            var members = db.Member.OrderBy(x => x.memberID);
            data.TotalCount = members.Count();
            data.TotalPage = (int)Math.Ceiling((decimal)data.TotalCount / (decimal)10);
            data.Data = members.Skip((page - 1) * 10).Take(10).ToList();
            data.CurrentPage = page;
            if (!string.IsNullOrEmpty(searchString))
            {
                // 使用 Contains 方法進行模糊搜尋，你可以根據實際需求調整查詢條件
                member = member.Where(c => c.name.Contains(searchString)).ToList();
            }
            return View(data);

            
            
        }

        public ActionResult MCreate()
        {
            return View();
        }

        [HttpPost]

        public ActionResult MCreate(Models.Member member)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Error = false;
                var temp = db.Member.Where(m => m.memberID == member.memberID).FirstOrDefault();
                if (temp != null)
                {
                    ViewBag.Error = true;
                    return View(member);
                }
                db.Member.Add(member);
                db.SaveChanges();
                return RedirectToAction("MIndex");
            }
            return View(member);
        }

        public ActionResult MEdit(int ID)
        {
            var member = db.Member.Where(m => m.memberID == ID).FirstOrDefault();
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
                return RedirectToAction("MIndex");

            }
            return View(member);
        }
        public ActionResult MDelete(int ID)
        {
            var MEMBER = db.Member.Where(m => m.memberID == ID).FirstOrDefault();
            db.Member.Remove(MEMBER);
            db.SaveChanges();
            return RedirectToAction("MIndex");
        }
    }
}