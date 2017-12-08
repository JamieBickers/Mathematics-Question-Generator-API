using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MathematicsQuestionGeneratorAPI.Models.RandomNumberGenerators;
using MathematicsQuestionGeneratorAPI.Models.MailSenders;
using MathematicsQuestionGeneratorAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MathematicsQuestionGeneratorAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
            services.AddDbContext<QuestionGeneratorContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton(Configuration);
            services.AddSingleton<IRandomIntegerGenerator>(new RandomIntegerGenerator());
            services.AddTransient<IMailSender, SmtpMailSender>();
            services.AddSingleton<ILogger>((new LoggerFactory()).CreateLogger("general"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options.WithOrigins("http://jamiebickers.s3-website.eu-west-2.amazonaws.com", "*", "*").AllowAnyMethod().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseMvc();
        }
    }
}
