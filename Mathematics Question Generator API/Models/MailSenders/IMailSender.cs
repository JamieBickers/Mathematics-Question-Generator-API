using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.MailSenders
{
    public interface IMailSender
    {
        void SendEmail(string reciever, List<MemoryStream> streams);
    }
}
