using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Rent
{
    public class LinePay
    {
        private readonly string channelId = "2002023149";
        private readonly string channelSecret = "9559ada7e9a06ed8f500b04ff1995659";
        private readonly HttpClient httpClient = new HttpClient();

        public async Task<string> RequestPayment(decimal amount, string currency, string orderId)
        {
            var requestUri = "https://sandbox-api-pay.line.me/v2/payments/request";
            var nonce = Guid.NewGuid().ToString();
            var requestBody = new
            {
                amount = amount,
                currency = currency,
                orderId = orderId,
                packages = new List<object>
                {
                    new
                    {
                        id = "1",
                        amount = amount,
                        name = "Package Name",
                        products = new List<object>
                        {
                            new
                            {
                                name = "Product Name",
                                quantity = 1,
                                price = amount
                            }
                        }
                    }
                },
                redirectUrls = new
                {
                    confirmUrl = "/LinePay/confirm",
                    cancelUrl = "/LinePay/cancel"
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.Add("X-LINE-ChannelId", channelId);
            content.Headers.Add("X-LINE-ChannelSecret", channelSecret);
            content.Headers.Add("X-LINE-Authorization-Nonce", nonce);

            var response = await httpClient.PostAsync(requestUri, content);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}