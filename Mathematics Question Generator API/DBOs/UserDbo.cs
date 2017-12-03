using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathematicsQuestionGeneratorAPI.Models
{
    public class UserDbo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(60)]
        public string EmailAddress { get; set; }

        public UserDbo() { }

        public UserDbo(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}