using MathematicsQuestionGeneratorAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public class WorksheetQuestionDbo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public QuestionDbo Question { get; set; }

        [Required]
        public WorksheetDbo Worksheet { get; set; }

        [Required]
        public int QuestionNumber { get; set; }

        public WorksheetQuestionDbo() { }

        public WorksheetQuestionDbo(QuestionDbo question, WorksheetDbo worksheet, int questionNumber)
        {
            Question = question;
            Worksheet = worksheet;
            QuestionNumber = questionNumber;
        }
    }
}