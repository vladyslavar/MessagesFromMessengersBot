using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MessagesFromMessengersBot
{
    class ApiForTelegram
    {
        public void AddUserToTelegramDB(long id, string username)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://gutapi.ml/api/telegram/adduser/{id}/{username}");
            httpWebRequest.Method = "POST";
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }
    }
    
}
