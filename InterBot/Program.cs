using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace InterBot
{
    class Program
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("596056551:AAHF_u5VlopPp_v91WVnl-ng5ilGL4UuA9Q");

        private static Dictionary<int,string> UserSteps = new Dictionary<int, string>();


        static void Main(string[] args)
        {

            bot.OnMessage += Bot_OnMessage;

            bot.StartReceiving();

            Console.WriteLine("Bot Started");

            Console.ReadKey();
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {





                if(UserSteps.ContainsKey(e.Message.From.Id) && UserSteps[e.Message.From.Id] == "Cidade")
                {
                    if (Cities().Find(c => c.Contains(e.Message.Text) ) != null)
                    {
                        Send(e, e.Message.From.Username + "\n Escolha uma acomodação");
                        Send(e, Houses());
                        UserSteps[e.Message.From.Id] = "Acomodacao";
                    }
                    else
                    {
                        Send(e, "Comando Inválido. Escolha uma das cidades");
                        Send(e, Cities());
                    }
                }








                if (e.Message.Text.Contains("/start"))
                {
                    Send(e, "Ola " +  e.Message.From.Username + "\n Escolha uma cidade");
                    Send(e, Cities());
                    UserSteps.Add(e.Message.From.Id, "Cidade");
                }





                

              
            }
        }



        public static void Send(Telegram.Bot.Args.MessageEventArgs e, string msg)
        {
            bot.SendTextMessageAsync(e.Message.Chat.Id, msg);
        }

        public static void Send(Telegram.Bot.Args.MessageEventArgs e, List<string> msgList)
        {
            var msg = msgList.Aggregate((i, j) => i + "\n" + j);
            Send(e, msg);
        }

        public static List<string> Cities()
        {
            return new List<string>()
            {
                "Berlin - Alemanha - Alemão",
                "Quebec - Canadá - Francês",
                "Dublin - Irlanda - Inglês"
            };
        }

        public static List<string> Houses()
        {
            return new List<string>()
            {
                "Casa de Familia",
                "Hostel",
                "Flat"
            };
        }

    }

    public static class StringExtension
    {
        public static bool ContainsAny(this string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
    }
}
