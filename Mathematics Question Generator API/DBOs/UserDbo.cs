using System.ComponentModel.DataAnnotations.Schema;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public class UserDbo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string EmailAddress { get; set; }

        public UserDbo() { }

        public UserDbo(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}