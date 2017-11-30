using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace MathematicsQuestionGeneratorAPI.Models.MailSenders
{

    public class SmtpMailSender : IMailSender
    {
        private const string FromAddress = "randomizedmathematicsquestions@gmail.com";
        private const string Password = "Ga173VC0GW9z";

        public void SendEmail(string reciever, List<MemoryStream> streams)
        {
            var client = new SmtpClient();
            var message = new MailMessage(FromAddress, reciever)
            {
                Subject = "Worksheet",
                Body = "A pdf of the worksheet is attached."
            };

            if (streams != null)
            {
                foreach (var stream in streams)
                {
                    stream.Position = 0;
                    var attachment = new Attachment(stream, "Worksheet.pdf");
                    message.Attachments.Add(attachment);
                }
            }

            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(FromAddress, Password);

            client.Timeout = 100000;

            try
            {
                client.Send(message);
            }
            catch
            {
                //TODO: Logging here
            }
            finally
            {
                message.Dispose();
            }
        }
    }
}
