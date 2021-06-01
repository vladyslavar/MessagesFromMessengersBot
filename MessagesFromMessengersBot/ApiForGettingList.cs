using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessagesFromMessengersBot
{
    class ApiForGettingList
    {
        public List<string> GetContactsFromTwitter(long tg_id)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://gutapi.ml/api/twitter/getallusers/{tg_id}");
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            var response = streamReader.ReadToEnd();

            List<TwitterContact> contacts = new List<TwitterContact>();
            List<string> contactsToReturn = new List<string>();
            contacts = JsonConvert.DeserializeObject<List<TwitterContact>>(response);


            foreach (var contact in contacts)
            {
                string stringToAdd = $"{contact.id_twitter_contact}";
                contactsToReturn.Add(stringToAdd);
            }
            return contactsToReturn;
            
        }
        public List<string> GetContactsFromViber(long tg_id)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://gutapi.ml/api/viber/getallusers/{tg_id}");
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            var response = streamReader.ReadToEnd();

            List<ViberUs> contacts = new List<ViberUs>();
            List<string> contactsToReturn = new List<string>();
            contacts = JsonConvert.DeserializeObject<List<ViberUs>>(response);


            foreach (var contact in contacts)
            {
                string stringToAdd = $"{contact.Username}" + ": "+ $"{contact.Viber_id}";
                contactsToReturn.Add(stringToAdd);
            }
            return contactsToReturn;
        }
        public List<string> GetContactsFromReddit(long tg_id)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://gutapi.ml/api/redit/getallusers/{tg_id}");
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            var response = streamReader.ReadToEnd();

            var contacts = JsonConvert.DeserializeObject<List<string>>(response);
            List<string> contactsToReturn = new List<string>();

            foreach (var contact in contacts)
            {
                contactsToReturn.Add(contact);
            }

                return contactsToReturn;
        }
    }

    public class TwitterContact
    {
        public long id { get; set; }
        public long id_telegram_user { get; set; }
        public string id_twitter_contact { get; set; }
        public string username_twitter_contact { get; set; }
    }
    public class ViberUs
    {
        public string Username { get; set; }
        public string Viber_id { get; set; }
    }
}
