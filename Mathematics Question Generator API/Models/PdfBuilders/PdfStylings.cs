using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Models.PdfBuilders
{
    public static class PdfStylings
    {
        public static float MARGIN = 72f;
        public static BaseColor FONT_COLOR = BaseColor.Black;
        public static float FONT_SIZE_BODY = 12f;
        public static float FONT_SIZE_TITLE = 20f;
        public static Font FONT_BODY = FontFactory.GetFont(FontFactory.HELVETICA, FONT_SIZE_BODY, FONT_COLOR);
        public static Font FONT_TITLE = FontFactory.GetFont(FontFactory.HELVETICA, FONT_SIZE_TITLE, FONT_COLOR);
        public static int SPACE_AFTER_TITLE = 18;
        public static int SPACE_AFTER_INSTRUCTIONS = 10;
    }
}