﻿using MathematicsQuestionGeneratorAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public class WorksheetQuestionDbo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public QuestionDbo Question { get; set; }
        public WorksheetDbo Worksheet { get; set; }
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