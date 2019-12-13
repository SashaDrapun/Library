using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Library
{
    public static class MessageToEmail
    {
        public static async Task SendMessageAsync(string email, string message)
        {
            MailAddress myAdress = new MailAddress("LibraryOfTheChirch@gmail.com", "Библиотека");
            MailAddress toMyAdress = new MailAddress(email);

            MailMessage mailMessage = new MailMessage(myAdress, toMyAdress);

            mailMessage.Subject = "Смена пароля";

            mailMessage.Body = "<h1>" + "Вы пытаетесь сменить пароль" + "</h1>";
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("LibraryOfTheChirch@gmail.com", GetPassword());
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mailMessage);
        }
        public static void Message(string email, string message)
        {
 

        }
        
        

        private static string GetPassword()
        {
            return "templeOfTheEpiphany";
        }
    }


}
