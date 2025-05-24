using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Hangfire;
using Microsoft.Owin;
using Hangfire.SqlServer;
using System.Diagnostics;
using Rent.Models;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using sib_api_v3_sdk.Client;
using System.Data.Entity;

[assembly: OwinStartup(typeof(Rent.Startup))]
namespace Rent
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Hangfire with SQL Server storage
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\rent.mdf;Integrated Security=True");

            // Initialize the Hangfire server
            app.UseHangfireServer();

            // Map Dashboard to the `hangfire` route
            app.UseHangfireDashboard("/hangfire");

            // 設置定時任務
#pragma warning disable CS0618 // 類型或成員已經過時
            RecurringJob.AddOrUpdate(
                "send-email-reminder",
                () => new EmailReminderJob().Execute(),
                Cron.Minutely,
                TimeZoneInfo.Local
            );
            RecurringJob.AddOrUpdate(
                "check-rent-return",
                () => new RentReturnCheckJob().Execute(),
                Cron.Minutely, // 每小時檢查一次
                TimeZoneInfo.Local
            );
#pragma warning restore CS0618 // 類型或成員已經過時
        }
    }

    public class EmailReminderJob
    {
        public void Execute()
        {
            Debug.WriteLine("EmailReminderJob 開始執行。");
            try
            {
                using (var db = new rentEntities())
                {
                    var today = DateTime.Now;
                    // 直接在數據庫中進行日期比較和時間差計算
                    var orders = db.Order.Where(o => DbFunctions.TruncateTime(o.orderDate) > DbFunctions.TruncateTime(today)
                                                 && DbFunctions.DiffHours(o.orderDate, today) < 24
                                                 && !o.emailSent).ToList();
                    foreach (var order in orders)
                    {
                        var member = db.Member.Find(order.memberID);
                        string email = member.Email;
                        DateTime orderDate = order.orderDate;
                        string Name = member.name;
                        string orderID = order.orderID;
                        int rentC = order.rentC ?? 0;
                        SendReminderEmail(email,orderDate,Name,orderID, rentC);
                        order.emailSent = true;
                        // 更新訂單狀態的代碼...
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"發送提醒郵件時發生錯誤: {e.Message}");
            }
            Debug.WriteLine("EmailReminderJob 執行結束。");
        }


        public void SendReminderEmail(string email, DateTime orderDate, string Name,string orderID, int rentC)
        {
            

            // 創建發送郵件的實例
            var apiInstance = new TransactionalEmailsApi();
            
            var emailContent = $"TRent租車通知，提醒您，{Name}先生/小姐。<br>您的訂單{orderID}<br>將於{orderDate}進行租車，金額${rentC}元。<br>請於租車時間前12小時付款完畢，若未於時間付款則直接取消訂單。如您已繳費完成，請忽略此訊息。";
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
                    emailContent, // Customize your reminder email content
                    null,
                    "TRent租車提醒", // Customize your subject
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                );
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                
            }
            catch (Exception e)
            {
                // Handle error
                Debug.WriteLine($"發送提醒郵件時發生錯誤: {e.Message}");
            }
        }

    }
    public class RentReturnCheckJob
    {
        public void Execute()
        {
            Debug.WriteLine("RentReturnCheckJob 開始執行。");
            try
            {
                using (var db = new rentEntities())
                {
                    var today = DateTime.Now;
                    // 查找 returnDate 小於當前日期且 rentReturn 為 false 的訂單
                    var orders = db.Order.Where(o => DbFunctions.TruncateTime(o.returnDate) < DbFunctions.TruncateTime(today)
                                                 && !o.rentReturn).ToList();
                    foreach (var order in orders)
                    {
                        var member = db.Member.Find(order.memberID);
                        string email = member.Email;
                        DateTime returnDate = order.returnDate;
                        string Name = member.name;
                        string orderID = order.orderID;
                        int rentC = order.rentC ?? 0;
                        SendReminderEmail(email, returnDate, Name, orderID, rentC);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"檢查還車狀態時發生錯誤: {e.Message}");
            }
            Debug.WriteLine("RentReturnCheckJob 執行結束。");
        }

        public void SendReminderEmail(string email, DateTime returnDate, string Name, string orderID, int rentC)
        {
            

            // 創建發送郵件的實例
            var apiInstance = new TransactionalEmailsApi();

            var emailContent = $"TRent租車通知，提醒您，{Name}先生/小姐。<br>您的訂單{orderID}<br>已經過了還車時間 {returnDate}，請盡快還車。";
            // Setup email sender
            string senderName = "Takming Rent";
            string senderEmail = "80206jim@gmail.com";
            SendSmtpEmailSender emailSender = new SendSmtpEmailSender(senderName, senderEmail);

            // Setup email receiver
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(email);
            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo> { smtpEmailTo };

            // Send the email
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(
                    emailSender,
                    to,
                    null,
                    null,
                    emailContent, // Customize your reminder email content
                    null,
                    "TRent租車提醒", // Customize your subject
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                );
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
            }
            catch (Exception e)
            {
                // Handle error
                Debug.WriteLine($"發送提醒郵件時發生錯誤: {e.Message}");
            }
        }
    }
}

