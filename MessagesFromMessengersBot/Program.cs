using MessagesFromMessengersBot.ChekingIncomingMsgs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MessagesFromMessengersBot
{
    class Program
    {
        public static ITelegramBotClient botClient;
        public static List<long> users_ids = new List<long>();
        public static List<long> users_ids_reddit = new List<long>();
        public static List<long> users_ids_twitter = new List<long>();
        public static List<long> users_ids_twitter_first = new List<long>();
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("1837900345:AAE9DqlWWVHqTrfNEP7bxQD352A_2wl_XFM") { Timeout = TimeSpan.FromSeconds(20) };
            botClient.OnMessage += BotOnMessageReceived;
            botClient.StartReceiving();
            try
            {
                var me = botClient.GetMeAsync().Result;
            }
            catch (Exception exception) { Console.WriteLine(exception.Message); }

            
            do
            {

                ApiForReceivingMesssage receivingMesssage = new ApiForReceivingMesssage();

                List<ReceivedViberMsg> strs = receivingMesssage.ReceiveMessagesViber();
                foreach(var str in strs )
                {
                    if(Convert.ToInt64(str.receiver) != 0)
                    SendMessage(Convert.ToInt64(str.receiver), "Viber\n" + $"{str.sender}" + ": " + $"{str.text}");
                }
                ReddittoDelete delete = new ReddittoDelete();
                foreach(var user_id in users_ids_reddit)
                {
                    List<RedditMes> strsR = receivingMesssage.ReceiveMessagesReddit(user_id);
                    if(strsR != null)
                    {
                        foreach (var str in strsR)
                        {
                            if (str.Body == "TOKEN_EXPIRED")
                            {
                                delete.id = user_id;
                                SendMessage(user_id, "Reddit\n" + $"{str.Body}");
                            }
                            else
                            {
                                delete.id = 0;
                                SendMessage(user_id, "Reddit\n" + $"{str.Author}" + ": " + $"{str.Body}");
                            }
                            
                        }
                    }
                    
                }
                foreach(var user_id in users_ids_twitter)
                {
                    if (users_ids_twitter_first.Contains(user_id)) 
                    {
                        List<ReceivedMessagesTwitter> strsT = receivingMesssage.ReceiveMessagesTwitter(user_id);
                        users_ids_twitter_first.Clear();
                        continue; 
                    }
                    else
                    {
                        List<ReceivedMessagesTwitter> strsT = receivingMesssage.ReceiveMessagesTwitter(user_id);
                        if (strsT != null)
                        {
                            foreach (var str in strsT)
                            {
                                SendMessage(user_id, "Twitter\n" + $"{str.Sender_id}" + ": " + $"{str.Text}");
                            }
                        }
                    }
                    
                }

                if (delete.id != 0)
                {
                    users_ids_reddit.Remove(delete.id);

                }

                Thread.Sleep(20000);
           }
            while (true);
            /*
            //botClient.OnMessage += BotOnMessageReceived;
            Console.ReadKey();
            botClient.StopReceiving();*/
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            string pattern = @"[tvr][_][i][d][:]";
            string pattern2 = @"[v][_][l][:]";
            //string patternForToken = @"[k][e][y][1][:][a-z0-9]+\n[k][e][y][2][:][a-z0-9]+\n[k][e][y][3][:][a-z0-9]+";
            string patternForToken = @"(^[k][e][y][_][rt][:][a-z0-9]+)";
            Regex regex = new Regex(pattern);
            Regex regexForLast = new Regex(pattern2);
            Regex regexForToken = new Regex(patternForToken);
            if (regex.IsMatch(message.Text) || regexForLast.IsMatch(message.Text))
            {
                Regex regexT = new Regex(@"[t][_][i][d][:]");
                Regex regexV = new Regex(@"[v][_][i][d][:]");
                Regex regexR = new Regex(@"[r][_][i][d][:]");
                Regex regexV_last = new Regex(@"[v][_][l][:]");
                if(regexT.IsMatch(message.Text))
                {
                    Regex regexT2 = new Regex(@"[t][_][i][d][:][\s]?\w+");
                    var receiver = regexT2.Match(message.Text).Value;
                    var message_to_send = regexT2.Replace(message.Text, "");

                    if(new Regex(@"[t][_][i][d][:][\s]\w").IsMatch(message.Text))
                    {
                        string[] receivers = receiver.Split(": ");
                        receiver = receivers[1];
                    }
                    else
                    {
                        string[] receivers = receiver.Split(":");
                        receiver = receivers[1];
                    }
                    ApiForSendingMessages sendingMessages = new ApiForSendingMessages();
                    sendingMessages.SendMessageToTwitter(message.Chat.Id.ToString(), receiver, message_to_send);

                }
                else if(regexV.IsMatch(message.Text))
                {
                    Regex regexV2 = new Regex(@"[v][_][i][d][:][\s]?[\w+\d+=-]+");
                    var receiver = regexV2.Match(message.Text).Value;
                    var message_to_send = regexV2.Replace(message.Text, "");

                    if (new Regex(@"[v][_][i][d][:][\s]\w").IsMatch(message.Text))
                    {
                        string[] receivers = receiver.Split(": ");
                        receiver = receivers[1];
                    }
                    else
                    {
                        string[] receivers = receiver.Split(":");
                        receiver = receivers[1];
                    }
                    ApiForSendingMessages sendingMessages = new ApiForSendingMessages();
                    sendingMessages.SendMessageToViber(message.Chat.Username, receiver, message_to_send);
                }
                else if(regexR.IsMatch(message.Text))
                {
                    Regex regexR2 = new Regex(@"[r][_][i][d][:][\s]?\w+");
                    var receiver = regexR2.Match(message.Text).Value;
                    var message_to_send = regexR2.Replace(message.Text, "");


                    if (new Regex(@"[r][_][i][d][:][\s]\w").IsMatch(message.Text))
                    {
                        string[] receivers = receiver.Split(": ");
                        receiver = receivers[1];
                    }
                    else
                    {
                        string[] receivers = receiver.Split(":");
                        receiver = receivers[1];
                    }
                    ApiForSendingMessages sendingMessages = new ApiForSendingMessages();
                    sendingMessages.SendMessageToReddit(message.Chat.Id, receiver, message_to_send);
                }
                else if (regexV_last.IsMatch(message.Text))
                {
                    Regex regexV2 = new Regex(@"[v][_][l][:]");
                    var receiver = "last";
                    var message_to_send = regexV2.Replace(message.Text, "");

                    ApiForSendingMessages sendingMessages = new ApiForSendingMessages();
                    sendingMessages.SendMessageToViber(message.Chat.Username, receiver, message_to_send);
                }
            }

            else if (regexForToken.IsMatch(message.Text))
            {
                CheckTokens check = new CheckTokens();
                string res = check.Tokens(message.Text, message.Chat.Id);
                if(res == "Inappropriate format")
                {
                    await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: res);
                }
                else if(res == "reddit")
                {
                    if (!(users_ids_reddit.Contains(message.Chat.Id)))
                    {
                        users_ids_reddit.Add(message.Chat.Id);
                    }
                }
                else if (res == "twitter")
                {
                    if (!(users_ids_twitter.Contains(message.Chat.Id)))
                    {
                        users_ids_twitter.Add(message.Chat.Id);
                        users_ids_twitter_first.Add(message.Chat.Id);
                    }
                }

            }

            else if(message != null && (message.Type == MessageType.Text))
            {
                switch (message.Text.Split(' ').First())
                {
                    #region Main Commands
                    case "/start":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Bot to receive messages from twitter, viber and reddit\n/authorize first to receive messages");
                        ApiForTelegram apiForTelegram = new ApiForTelegram();
                        if(!(users_ids.Contains(message.Chat.Id)))
                        {
                            apiForTelegram.AddUserToTelegramDB(message.Chat.Id, message.Chat.Username);
                            users_ids.Add(message.Chat.Id);
                        }
                        break;

                    case "/authorize":

                        if(!users_ids.Contains(message.Chat.Id))
                        {
                            users_ids.Add(message.Chat.Id);
                        }
                        await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Choose messenger to authorize",
                        replyMarkup: buttons1()
                        );
                        //keyboard with messaengers
                        break;

                    case "/send":
                        if (!users_ids.Contains(message.Chat.Id))
                        {
                            users_ids.Add(message.Chat.Id);
                        }
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Choose messenger to send message",
                            replyMarkup: buttons2());
                        //keyboard with messaengers
                        break;

                    case "/getlist":
                        if (!users_ids.Contains(message.Chat.Id))
                        {
                            users_ids.Add(message.Chat.Id);
                        }
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Choose messenger to get list of contacts",
                            replyMarkup: buttons3());
                        //keyboard with messaengers
                        break;
                    #endregion
                    #region GetMessages
                    case "Twitter_Get":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Folow this link to authorize to Twitter and receive messages\n" + 
                            $"https://gutapi.ml/api/twitter/authorizeanoth"
                            +"\nThen send received key in format:\n" +
                            "key_t:{here your key}\n\n" + "Notice: You need be logined in Twitter on your device");
                        break;

                    case "Viber_Get":
                        if (!(users_ids.Contains(message.Chat.Id)))
                        {
                            users_ids.Add(message.Chat.Id);
                        }
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Viber messages will be delivered to you");
                        break;

                    case "Reddit_Get":
                        string appId = "";
                        string appSecret = "";

                        

                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Folow this link to authorize to Reddit and receive messages\n" + 
                            $"https://gutapi.ml/api/redit/authorizeanoth"
                            + "\nThen send received key in format:\n" +
                            "key_r:{here your key}\n\n" + "Notice: You need be logined in Reddit on your device");
                        break;
                    #endregion
                    #region SendMessages
                    case "Twitter_Send":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "send message in format:\n\n t_id:twitter_receiver_id any message\n" +
                            "To get ids of your twitter contacts use /getlist");
                        break;
                    case "Viber_Send":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "send message in format:\n\nv_id:viber_receiver_id any message\nv_l:message (message will be delivered to previous receiver)\n\n" +
                            "To get ids of your viber contacts use /getlist");
                        
                        break;
                    case "Reddit_Send":
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "send message in format:\n\n r_id:reddit_receiver_id any message\n" +
                            "To get ids of your reddit contacts use /getlist");
                        
                        break;
                    #endregion
                    #region GetList
                    case "Twitter_List":
                        
                        ApiForGettingList gettingList = new ApiForGettingList();
                        List<string> results = new List<string>();
                        results = gettingList.GetContactsFromTwitter(message.Chat.Id);
                        string ids = ReceivingUsers(results);
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"Your Twitter contacts ids:\n{ids}");
                        break;

                    case "Viber_List":
                        ApiForGettingList gettingList1 = new ApiForGettingList();
                        List<string> results1 = new List<string>();
                        results1 = gettingList1.GetContactsFromViber(message.Chat.Id);
                        string ids1 = ReceivingUsers(results1);
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"Your Viber contacts ids:\n{ids1}");
                        break;

                    case "Reddit_List":

                        ApiForGettingList gettingList2 = new ApiForGettingList();
                        List<string> results2 = new List<string>();
                        results2 = gettingList2.GetContactsFromReddit(message.Chat.Id);
                        string ids2 = ReceivingUsers(results2);
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"Your Reddit contacts ids:\n{ids2}");
                        break;
                        #endregion
                }
            }
        }

        static IReplyMarkup buttons1()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Twitter_Get"}, },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Viber_Get"}, },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Reddit_Get"}, },
                }
            };
        }

        static IReplyMarkup buttons2()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Twitter_Send"},},
                    new List<KeyboardButton> { new KeyboardButton { Text = "Viber_Send"}, },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Reddit_Send"}, },
                }
            };
        }

        static IReplyMarkup buttons3()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Twitter_List"},},
                    new List<KeyboardButton> { new KeyboardButton { Text = "Viber_List"},  },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Reddit_List"}, },
                }
            };
        }
        static string ReceivingUsers(List<string> results)
        {
            string ids = "";
            foreach (var result in results)
            {
                ids = ids + $"{result}\n";
            }
            return ids;
        }
        private static async void SendMessage(long id, string message)
        {
            await botClient.SendTextMessageAsync(
                            chatId: id,
                            text: message);
        }
    }
    class ReddittoDelete
    {
        public long id { get; set; }
    }
}
