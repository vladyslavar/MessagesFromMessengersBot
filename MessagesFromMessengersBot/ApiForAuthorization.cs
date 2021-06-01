using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MessagesFromMessengersBot
{
    class ApiForAuthorization
    {
        public void AuthorizeInTwitter(long id_tg, string access_token)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create
                ($"https://gutapi.ml/api/twitter/addtokens/{id_tg}/{access_token}");
                httpWebRequest.Method = "POST";
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch(Exception e)
            {

            }
            
        }

        public void AuthorizeInReddit(long id_tg, string access_token)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create
                ($"https://gutapi.ml/api/redit/addtokens/{id_tg}/{access_token}");
            httpWebRequest.Method = "POST";
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }
    }
}
