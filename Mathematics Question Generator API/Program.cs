using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using MathematicsQuestionGeneratorAPI.Data;
using Microsoft.Extensions.DependencyInjection;

namespace MathematicsQuestionGeneratorAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);

            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<QuestionGeneratorContext>();
                    DbInitialiser.Initialise(context);
                }
                catch (Exception exception)
                {
                    //TODO: Logging here
                    throw exception;
                }
            }

            webHost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
