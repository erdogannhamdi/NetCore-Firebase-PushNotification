using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Notification.Model;

namespace Notification.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendNotificationController : ControllerBase
    {
        [HttpGet]
        [Route("send")]
        public void sendData(){
            for(int i = 0; i < 10; i++){
                
                String cultureName = "en-GB";
                var cultureInfo = new CultureInfo(cultureName);
                DateTime localDate = DateTime.Now;
                
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var objNotification = new
                {
                    to = "Fcm_Token",
                    notification = new
                    {
                        //title = "notification: " + (i+1),
                        title = localDate.ToString("HH:mm:ss"),
                        body = "body"
                        //icon = "/image/accept.png"
                    },
                    data = new 
                    {
                        bilgi = "id : " + (i + 1)
                    }
                };
                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", "Server_API_key"));
                tRequest.Headers.Add(string.Format("Sender: id={0}", "Sender_Id"));
                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                Console.WriteLine(jsonNotificationFormat);
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();

                                FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                                if (response.success == 1)
                                {

                                    Console.WriteLine("succeeded");
                                }
                                else if (response.failure == 1)
                                {
                                Console.WriteLine("failed");

                                }

                            }
                        }

                    }
                }
            } 
            
        }     
        
    }
}
