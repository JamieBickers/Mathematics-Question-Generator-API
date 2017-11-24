using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MailSenders
{

    public class SmtpMailSender
    {
        private static string password = "Ga173VC0GW9z";

        public void SendEmail(string reciever, List<MemoryStream> streams = null)
        {
            var client = new SmtpClient();
            var message = new MailMessage();

            message.From = new MailAddress(reciever);
            message.To.Add("bickersjamie@googlemail.com");
            message.Subject = "my subject";
            message.Body = "my body";

            if (streams != null)
            {
                foreach (var stream in streams)
                {
                    stream.Position = 0;
                    var attachment = new Attachment(stream, "Test.pdf");
                    message.Attachments.Add(attachment);
                }
            }

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
            catch
            {
                Console.WriteLine("Error");
            }
            finally
            {
                message.Dispose();
            }
        }
    }
}
