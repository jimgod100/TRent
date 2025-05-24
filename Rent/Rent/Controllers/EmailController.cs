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

namespace Rent.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        rentEntities db = new rentEntities();
        public async Task<ActionResult> Forget(string email)
        {
            var member = await GetMemberByEmailAsync(email);

            if (member != null)
            {
                // 如果會員存在，將 email 存入 TempData，稍後在 Index 方法中使用
                TempData["ForgetEmail"] = email;

                // 重定向到 Index 方法
                return RedirectToAction("Index");
            }
            else
            {
                // 如果找不到會員，添加錯誤訊息
                ModelState.AddModelError("EmailNotFound", "找不到相應的會員");
                return View();
            }

            
        }
        public ActionResult Index()
        {
            
                // 設定 Sendinblue API 金鑰
            

            var apiInstance = new TransactionalEmailsApi();

            string ToEmail = TempData["ForgetEmail"] as string;

            if (string.IsNullOrEmpty(ToEmail))
            {
                // 如果 ToEmail 為空，可能是直接訪問 Index 而不是經由 Forget，這裡可以根據你的需求進行處理
                // 這裡的例子是直接返回一個 View，你可以根據需求修改
                return View("Forget");
            }

            string SenderName = "Takming Rent";
            string SenderEmail = "80206jim@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);

            string ToName = "John Doe";
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);

            // 其他 Sendinblue 程式碼...

            try
            {
                // 其他 Sendinblue 程式碼...
                var sendSmtpEmail = new SendSmtpEmail(
                    Email,        // 寄件人
                    To,            // 收件人列表
                    null,          // 密件副本收件人列表
                    null,          // 副本收件人列表
                    "https://localhost:44380/Email/Edit",   // HTML 格式的郵件內容
                    null,          // 純文本格式的郵件內容
                    "忘記密碼",       // 郵件主題
                    null,       // 回覆地址
                    null,   // 附件列表
                    null,       // 自定義標頭
                    null,    // 郵件模板的 ID
                    null,    // 模板參數
                    null,          // 多版本郵件內容
                    null           // 郵件標籤
                    );
                    
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);

                Debug.WriteLine(result.ToJson());
                Console.WriteLine(result.ToJson());

                // 其他處理成功情況的邏輯...

                return RedirectToAction("Success");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);

                // 其他處理錯誤情況的邏輯...

                return View();
            }

            
        }
        private async Task<Models.Member> GetMemberByEmailAsync(string email)
        {
            // 在實際應用中，這裡應該是使用資料庫查詢，而不是使用 Session
            // 這裡僅作為示例，使用非同步資料庫查詢提高並發性能
            return await db.Member.FirstOrDefaultAsync(m => m.Email == email);
        }
        public ActionResult Success()
        {
            return RedirectToAction("Login", "Login");
        }
        public ActionResult Edit()
        {
            string email = TempData["ForgetEmail"] as string;
            var member = db.Member.Where(m => m.Email == email).FirstOrDefault();
            return View(member);
        }

        [HttpPost]

        public ActionResult Edit(Models.Member member, string ConfirmPasswd)
        {

            if (ModelState.IsValid)
            {
                var existingMember = db.Member.Where(m => m.memberID == member.memberID).FirstOrDefault();

                // 驗證舊密碼
                if (member.Passwd != ConfirmPasswd)
                {
                    ModelState.AddModelError("ConfirmPasswd", "新密碼和確認密碼不相符。");
                    return View(member);
                }

                // 更新會員資料
                existingMember.name = member.name;
                existingMember.Passwd = member.Passwd;
                existingMember.ID = member.ID;
                existingMember.Gender = member.Gender;
                existingMember.Birthday = member.Birthday;
                existingMember.licenses = member.licenses;
                existingMember.Phone = member.Phone;
                existingMember.Email = member.Email;
                existingMember.emgyContact = member.emgyContact;
                existingMember.relationship = member.relationship;
                existingMember.emgyPhone = member.emgyPhone;
                existingMember.bDate = member.bDate;
                db.SaveChanges();

                return RedirectToAction("Login", "Login");


            }
            return View(member);

        }
    }
}