using SMTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySMTP
{
    class Program
    {
        static void Main(string[] args)
        {
            var smtp = new SmtpConnection();
            smtp.Connect("smtp.mail.ru", 587);

            smtp.ExtendedHello("bla-bla");
            smtp.StartTls("mail.ru");

            smtp.ExtendedHello("bla-bla");
            smtp.AuthPlain("login", "password");

            smtp.Mail("login@mail.ru");
            smtp.Recipient("reciever");
            //smtp.Data(EmailFormatter.GetText("login@mail.ru", "Subject", "reciever",null,"Hello"));
            smtp.Data(EmailFormatter.GetHtml("login@mail.ru", "Subject", "reciever",null, " < h1>Hello!</h1> <a href='stackoverflow.com'>click!</a>"));

            smtp.Quit();
        }
    }
}
