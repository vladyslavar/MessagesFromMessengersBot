using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessagesFromMessengersBot
{
    class ApiForSendingMessages
    {
        public void SendMessageToTwitter(string id_sender_tg, string id_receiver_twitter, string text)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create
                ($"https://gutapi.ml/api/twitter/sendmessages/{id_sender_tg}/{id_receiver_twitter}/{text}");
            httpWebRequest.Method = "POST";
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }
        public void SendMessageToViber(string username_sender_tg, string id_receiver_viber, string text)
        {
            HttpResponseMessage response;
            ViberSendMessage message = new ViberSendMessage()
            {
                Sender_username = username_sender_tg,
                Receiver_id = id_receiver_viber,
                Message = text
            };
            string json = JsonConvert.SerializeObject(message);

            using (var client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("POST"), "https://gutapi.ml/api/viber/sendmessages");
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");   // This is where your content gets added to the request body

                response = client.SendAsync(requestMessage).Result;
            }
        }
        public void SendMessageToReddit(long id_tg, string receiver, string text)
        {
            string subject = "New Message";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create
                ($"https://gutapi.ml/api/redit/sendmessage/{id_tg}/{subject}/{receiver}/{text}");
            httpWebRequest.Method = "POST";
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }

        public class ViberSendMessage
        {
            public string Sender_username { get; set; }
            public string Receiver_id { get; set; }
            public string Message { get; set; }
        }
    }
}
