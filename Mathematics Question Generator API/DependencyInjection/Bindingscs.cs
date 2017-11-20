using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using Ninject;
using Ninject.Modules;

namespace MathematicsQuestionGeneratorAPI.DependencyInjection
{
    public class Bindingscs : NinjectModule
    {
        public override void Load()
        {
            Bind<IRandomIntegerGenerator>().To<RandomIntegerGenerator>();
        }
    }
}
