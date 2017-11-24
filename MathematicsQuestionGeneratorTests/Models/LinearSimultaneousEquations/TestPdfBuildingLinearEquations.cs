using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MathematicsQuestionGeneratorAPI.Models.MathematicalModels.SimultaneousEquations;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models.PdfBuilders;
using MathematicsQuestionGeneratorAPI.Models;

namespace MathematicsQuestionGeneratorTests.Models.SimultaneousEquations
{
    [TestClass]
    public class TestPdfBuildingLinearEquations
    {
        private static string saveLocation = @"C:\Users\Jamie\Desktop\MathematicsQuestionGeneratorAPI\Worksheets";

        [TestMethod]
        public void MakePdf()
        {
            var integerGenerator = new FixedRandomIntegerGenerator(153);
            var questionGenerator = new LinearSimultaneousEquationsGenerator(integerGenerator);
            var title = "Simultaneous Equations";
            var instructions = "Solve the equations below, giving your answers to 2 decimal places";
            var questions = new List<IQuestion>();

            for (int i = 0; i < 10; i++)
            {
                questions.Add(questionGenerator.GenerateQuestionAndAnswer());
            }

            var pdfBuilder = new BasicPdfBuilder(questions, title, instructions);
            pdfBuilder.CreatePdfsAndSaveLocallyAsFiles(saveLocation);
        }
    }
}
