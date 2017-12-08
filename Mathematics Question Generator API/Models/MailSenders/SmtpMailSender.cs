using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace MathematicsQuestionGeneratorAPI.Models.MailSenders
{

    public class SmtpMailSender : IMailSender
    {
        private readonly string FromAddress;
        private readonly string Password;
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public SmtpMailSender(IConfiguration configuration, ILogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            FromAddress = configuration["EmailSettings:FromAddress"];
            Password = configuration["Emailsettings:Password"];
        }

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

            client.Host = configuration["Emailsettings:Host"];
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(FromAddress, Password);

            client.Timeout = 100000;

            try
            {
                client.Send(message);
            }
            catch (Exception exception)
            {
                logger.LogError("Error sending email", new object[] { exception, reciever, streams.Count });
            }
            finally
            {
                message.Dispose();
            }
        }
    }
}
