using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Rent;
using Rent.Domain;
using Rent.Dtos;
using Rent.Providers;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Rent.Controllers
{
    public class LinePayController : Controller
    {
        private readonly LinePayService _linePayService = new LinePayService();

        [HttpPost]
        public async Task<ActionResult> Create(PaymentRequestDto dto)
        {
            var response = await _linePayService.SendPaymentRequest(dto);
            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> Confirm(string transactionId, string orderId, PaymentConfirmDto dto)
        {
            var response = await _linePayService.ConfirmPayment(transactionId, orderId, dto);
            return Json(response);
        }

        [HttpGet]
        public ActionResult CancelTransaction(string transactionId)
        {
            _linePayService.TransactionCancel(transactionId);
            return RedirectToAction("Index"); // Redirect to the appropriate action
        }
    }
}