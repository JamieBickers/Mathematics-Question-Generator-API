using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MailSenders
{

    public class SmtpMailSender
    {
        private static string password = "ENTER_PASSWORD_HERE";

        public void SendEmail()
        {
            var client = new SmtpClient();
            var message = new MailMessage();

            message.From = new MailAddress("randomizedmathematicsquestions@gmail.com");
            message.To.Add("bickersjamie@googlemail.com");
            message.Subject = "my subject";
            message.Body = "my body";

            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("randomizedmathematicsquestions@gmail.com", password);

            client.Timeout = 100000;

            try
            {
                client.Send(message);
                Console.WriteLine("Sent");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
            }
        }
    }
}
