using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RegIN_Filimonova.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message, string To)
        {
            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("alenafilimonovafilimonowa@yandex.ru", "uphnsxzyigrfgsrx"),
                EnableSsl = true
            };
            smtpClient.Send("alenafilimonovafilimonowa@yandex.ru", To, "Проект RegIn", Message);
        }
    }
}
