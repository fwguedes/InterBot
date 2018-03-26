using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;

namespace InterBot
{
    class Program
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("596056551:AAHF_u5VlopPp_v91WVnl-ng5ilGL4UuA9Q");

        private static Dictionary<int, string> UserSteps = new Dictionary<int, string>();

        private static Agendamento agendamento;


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

                //Contato Inicial ou Reiniciando o bot
                if (e.Message.Text.Contains("/start"))
                {
                    Send(e, "Ola " + e.Message.From.Username + "\n Escolha uma cidade");
                    Send(e, Cities());
                    if (!UserSteps.ContainsKey(e.Message.From.Id))
                    {
                        UserSteps.Add(e.Message.From.Id, "Cidade");
                    }
                    else
                    {
                        UserSteps[e.Message.From.Id] = "Cidade";
                    }

                    agendamento = new Agendamento();
                    agendamento.Id = Guid.NewGuid();
                    agendamento.Nome = e.Message.From.Username;

                }
                else
                {
                    switch (UserSteps[e.Message.From.Id])
                    {

                        case "Cidade":


                            if (Cities().Find(c => c.Contains(e.Message.Text)) != null)
                            {
                                agendamento.Cidade = e.Message.Text;

                                Send(e, e.Message.From.Username + "\n Escolha uma acomodação");
                                Send(e, Houses());
                                UserSteps[e.Message.From.Id] = "Acomodação";

                            }
                            else
                            {
                                Send(e, "Comando Inválido. Escolha uma das cidades");
                                Send(e, Cities());
                            }

                            break;

                        case "Acomodação":


                            if (Houses().Find(c => c.Contains(e.Message.Text)) != null)
                            {
                                agendamento.Acomodação = e.Message.Text;

                                Send(e, e.Message.From.Username + "\n Escolha uma escola");
                                Send(e, Schools());
                                UserSteps[e.Message.From.Id] = "Escola";

                            }
                            else
                            {
                                Send(e, "Comando Inválido. Escolha uma das acomodações");
                                Send(e, Houses());
                            }

                            break;

                        case "Escola":


                            if (Schools().Find(c => c.Contains(e.Message.Text)) != null)
                            {
                                agendamento.Escola = e.Message.Text;
                                
                                Send(e, e.Message.From.Username + "\n Escolha uma dos adicionais");
                                Send(e, Adds());
                                UserSteps[e.Message.From.Id] = "Adicional";

                            }
                            else
                            {
                                Send(e, "Comando Inválido. Escolha uma das escolas");
                                Send(e, Schools());
                            }

                            break;

                        case "Adicional":


                            if (Adds().Find(c => c.Contains(e.Message.Text)) != null)
                            {
                                agendamento.Adicionais = e.Message.Text;

                                Send(e, e.Message.From.Username + "\n Digite seu tempo de permanencia em dias, meses ou anos");
                                UserSteps[e.Message.From.Id] = "Tempo";

                            }
                            else
                            {
                                Send(e, "Comando Inválido. Escolha uma dos adicionais");
                                Send(e, Adds());
                            }

                            break;

                        case "Tempo":


                            if (IsValidTime(e.Message.Text))
                            {
                                agendamento.Tempo = e.Message.Text;

                                Send(e, "Solicitação de Intercambio\n-Cidade: " + agendamento.Cidade + "\n-Acomodação: " + agendamento.Acomodação + "\n-Escola: " + agendamento.Escola + "\n-Adicional: " + agendamento.Adicionais + "\n-Tempo de Permanencia: " + agendamento.Tempo);
                                Send(e, e.Message.From.Username + "\n Confirmar Solicitação ?(Sim/Não)");
                                UserSteps[e.Message.From.Id] = "Confirmação";

                            }
                            else
                            {
                                Send(e, "Comando Inválido. Digite seu tempo de permanencia em dias, meses ou anos");
                                Send(e, Adds());
                            }

                            break;

                        case "Confirmação":


                            if (e.Message.Text == "Sim")
                            {
                                Send(e, "Diga quando você virá a agencia");
                                UserSteps[e.Message.From.Id] = "Agendamento";

                            }
                            else if (e.Message.Text == "Não")
                            {
                                agendamento = new Agendamento();
                                agendamento.Id = Guid.NewGuid();
                                agendamento.Nome = e.Message.From.Username;

                                Send(e, "Reiniciando...Escolha a cidade");
                                Send(e, Cities());
                                UserSteps[e.Message.From.Id] = "Cidade";

                                

                            }
                            else
                            {
                                Send(e, "Comando Inválido. Confirmar Solicitação ?(Sim/Não)");
                            }

                            break;



                        case "Agendamento":

                            if (IsValidDateTimeTest(e.Message.Text))
                            {
                                Send(e, "Visita Agendada!");
                                UserSteps[e.Message.From.Id] = "Finalizado";
                                agendamento.VisitaNaAgencia = e.Message.Text;

                            }
                            else
                            {
                                Send(e, "Data Invalida");

                            }

                            break;



                        default:
                            break;
                    }
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

        public static List<string> Schools()
        {
            return new List<string>()
            {
                "Internetional House - Premium",
                "Evanz - Normal",
                "Thompsom - Budget"
            };
        }

        public static List<string> Adds()
        {
            return new List<string>()
            {
                "Transfer",
                "Workshop",
                "Nenhum"
            };
        }


        public static bool IsValidTime(string time)
        {
            var regex = new Regex("[0-9]* [a-z|A-Z]*");
            return regex.Match(time).Success;

        }

        public static bool IsValidDateTimeTest(string dateTime)
        {
            string[] formats = { "dd/MM/yyyy" };
            DateTime parsedDateTime;
            return DateTime.TryParseExact(dateTime, formats, new CultureInfo("pt-BR"),
                                           DateTimeStyles.None, out parsedDateTime);
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
