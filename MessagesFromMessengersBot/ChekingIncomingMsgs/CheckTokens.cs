using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MessagesFromMessengersBot.ChekingIncomingMsgs
{
    public class CheckTokens
    {
        ApiForAuthorization authorization = new ApiForAuthorization();
        public string Tokens(string message, long id)
        {
            Regex regexT = new Regex(@"(^[k][e][y][_][t][:][\w+\d+=-]+)");
            Regex regexR = new Regex(@"(^[k][e][y][_][r][:][\w+\d+=-]+)");
            if(regexT.IsMatch(message))
            {
                var keyTwitter = regexT.Match(message).Value;
                string[] keys = keyTwitter.Split(":");
                keyTwitter = keys[1];

                authorization.AuthorizeInTwitter(id, keyTwitter);
                return "twitter";
            }
            else if(regexR.IsMatch(message))
            {
                //var keyReddit = regexT.Match(message).Value;
                string[] keys = message.Split(":");
                var keyReddit = keys[1];

                authorization.AuthorizeInReddit(id, keyReddit);
                return "reddit";
            }
            else
            {
                return "Inappropriate format";
            }
        }
    }
}
