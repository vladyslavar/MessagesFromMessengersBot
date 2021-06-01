using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MessagesFromMessengersBot
{
    class ApiForReceivingMesssage
    {
        public List<ReceivedViberMsg> ReceiveMessagesViber()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://gutapi.ml/api/viber/getmessage");
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            var response = streamReader.ReadToEnd();

            List<ReceivedViberMsg> msgs = new List<ReceivedViberMsg>();
            msgs = JsonConvert.DeserializeObject<List<ReceivedViberMsg>>(response);

            return msgs;
            
        }
        public List<RedditMes> ReceiveMessagesReddit(long id_tg)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://gutapi.ml/api/redit/getmessage/{id_tg}");
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            var response = streamReader.ReadToEnd();

            List<RedditMes> msgs = new List<RedditMes>();
            msgs = JsonConvert.DeserializeObject<List<RedditMes>>(response);

            return msgs;
        }
        public List<ReceivedMessagesTwitter> ReceiveMessagesTwitter(long id_receiver_tg)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://gutapi.ml/api/twitter/getmessages/{id_receiver_tg}");
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            var response = streamReader.ReadToEnd();

            List<ReceivedMessagesTwitter> msgs = new List<ReceivedMessagesTwitter>();
            msgs = JsonConvert.DeserializeObject<List<ReceivedMessagesTwitter>>(response);

            return msgs;
        }
    }

    public class ReceivedViberMsg
    {
        public string id { get; set; }
        public string text { get; set; }
        public string receiver { get; set; }
        public string sender { get; set; }
    }
    public class RedditMes
    {
        public string Body { get; set; }
        public string Author { get; set; }
        public string Context { get; set; }
    }

    public class ReceivedMessagesTwitter
    {
        public string Text { get; set; }
        public string Sender_id { get; set; }
        public string Receiver_id { get; set; }
    }
}
