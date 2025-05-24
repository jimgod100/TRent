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
using SendGrid.Helpers.Mail.Model;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Web.Helpers;
using Hangfire;
namespace Rent.Controllers
{
    public class RemindController : Controller
    {
        // GET: Remind
        rentEntities db = new rentEntities();
        public ActionResult Create(string rentID, string returnID, DateTime orderDate, string orderTime, DateTime returnDate, string returnTime, string carID)
        {
            // Check if order time and return time are the same
            DateTime combinedOrderDateTime = DateTime.Parse(orderDate.ToString("yyyy-MM-dd") + " " + orderTime);
            DateTime combinedReturnDateTime = DateTime.Parse(returnDate.ToString("yyyy-MM-dd") + " " + returnTime);
            if (combinedOrderDateTime.TimeOfDay == combinedReturnDateTime.TimeOfDay)
            {
                ViewBag.Message = $"時間不可相同";
                return View();
            }

            // Calculate time difference
            TimeSpan timeDifference = combinedOrderDateTime - DateTime.Now;

            // If time difference is less than 24 hours, schedule reminder email
            if (timeDifference.TotalHours < 24)
            {
                // Schedule reminder email using Hangfire
                BackgroundJob.Schedule(() => SendReminderEmail((Session["Member"] as Models.Member).Email), TimeSpan.FromHours(24 - timeDifference.TotalHours));

                ViewBag.Message = "提醒郵件已排程";

                // You can choose to return to a specific view here if needed
                return View();
            }

            // Return to a different action based on your logic
            return RedirectToAction("SomeAction", "SomeController");
        }

        private void SendReminderEmail(string email)
        {
            // Setup email sender
            string senderName = "Takming Rent";
            string senderEmail = "80206jim@gmail.com";
            SendSmtpEmailSender emailSender = new SendSmtpEmailSender(senderName, senderEmail);

            // Setup email receiver
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(email);
            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo> { smtpEmailTo };

            // Other email setup...

            // Send the email
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(
                    emailSender,
                    to,
                    null,
                    null,
                    "Your reminder email content here", // Customize your reminder email content
                    null,
                    "Your Subject Here", // Customize your subject
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                );

                var apiInstance = new TransactionalEmailsApi();
                apiInstance.SendTransacEmail(sendSmtpEmail);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);

                // Handle error
            }
        }


    }

}