using MathematicsQuestionGeneratorAPI.DBOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public class QuestionDbo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int SpecificQuestionID { get; set; }
        public QuestionTypeDbo QuestionType { get; set; }

        public QuestionDbo(int specificQuestionID, QuestionTypeDbo questionType)
        {
            SpecificQuestionID = specificQuestionID;
            QuestionType = questionType;
        }
    }
}