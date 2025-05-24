using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualBasic;
using LineBotSDK;
using LineBot;



namespace Rent.Controllers
{
    public class LineAccountBookController : ApiController
    {
        // GET: LineAccountBook
        [System.Web.Http.HttpPost]
        public IHttpActionResult POST(HttpRequestMessage request)
        {
            // 設定你的Channel Access Token
            string ChannelAccessToken = "YOUR_CHANNEL_ACCESS_TOKEN";
            isRock.LineBot.Bot bot;

            // 如果有Web.Config app setting，以此優先
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("LineChannelAccessToken"))
            {
                ChannelAccessToken = System.Configuration.ConfigurationManager.AppSettings["LineChannelAccessToken"];
            }

            // create bot instance
            bot = new isRock.LineBot.Bot(ChannelAccessToken);

            try
            {
                // 取得 http Post RawData(should be JSON)
                string postData = request.Content.ReadAsStringAsync().Result;
                // 剖析JSON
                var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);
                var UserSays = ReceivedMessage.events[0].message.text;
                var ReplyToken = ReceivedMessage.events[0].replyToken;
                // 依照用戶說的特定關鍵字來回應
                switch (UserSays.ToLower())
                {
                    case "/teststicker":
                        // 回覆貼圖
                        bot.ReplyMessage(ReplyToken, 1, 1);
                        break;
                    case "/testimage":
                        // 回覆圖片
                        bot.ReplyMessage(ReplyToken, new Uri("YOUR_IMAGE_URL"));
                        break;
                    default:
                        // 回覆訊息
                        string Message = "哈囉, 你說了:" + UserSays;
                        // 回覆用戶
                        bot.ReplyMessage(ReplyToken, Message);
                        break;
                }
                // 回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }
    }
}