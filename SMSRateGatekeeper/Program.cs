using Newtonsoft.Json;
using SMSRateGatekeeper.Abstractions;
using SMSRateGatekeeper.Options;
using SMSRateGatekeeper.Services;

namespace SMSRateGatekeeper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            var config = builder.Configuration;

            // Add services to the container.

            services.Configure<SMSProviderOptions>(builder.Configuration.GetSection("SMSProvider"))
                    .AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings
                               .ReferenceLoopHandling = ReferenceLoopHandling.Ignore;                 
                    });
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer()
                    .AddSwaggerGen();

            services.AddScoped<IMessageSendingService, MessageSendingService>()
                    .AddScoped<ISMSProviderService, SMSProviderService>()
                    .AddSingleton<IProviderLimitsGuard, ProviderLimitsGuard>();

           var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
