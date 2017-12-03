using MathematicsQuestionGeneratorAPI.DBOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public class QuestionDbo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int SpecificQuestionID { get; set; }

        [Required]
        public QuestionTypeDbo QuestionType { get; set; }

        public QuestionDbo() { }

        public QuestionDbo(int specificQuestionID, QuestionTypeDbo questionType)
        {
            SpecificQuestionID = specificQuestionID;
            QuestionType = questionType;
        }
    }
}