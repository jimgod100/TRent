using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Rent.Models;

namespace Rent.Controllers
{
    public class DbController : Controller
    {
        // GET: Db
        private readonly rentEntities _context;
        public DbController(rentEntities context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult GetProduct(string productId)
        {
            var product = _context.Order.FirstOrDefault(p => p.orderID == productId);
            if (product == null)
            {
                return HttpNotFound();
            }
            return Json(product, JsonRequestBehavior.AllowGet);
        }
    }
}