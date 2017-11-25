using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Exceptions
{
    // Throw these when something happens that should be impossible, indicating an error in the code rather than from the user.
    public class MathematicalImpossibilityException : Exception { }
}
