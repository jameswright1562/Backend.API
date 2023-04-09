using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Backend.Interfaces;
using Backend.Tokens;


[assembly: FunctionsStartup(typeof(Backend.API.Startup))]
namespace Backend.API;

public class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder config) 
    {
        var context = config.GetContext();
        var appPath = context.ApplicationRootPath;

        config.ConfigurationBuilder
            .AddJsonFile(Path.Combine(appPath, "settings.json"), optional: true, reloadOnChange: true)
            .AddJsonFile(Path.Combine(appPath, "local.settings.json"), optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
        
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddScoped<IValidator, Validator>();
        builder.Services.AddHttpClient();
    }
}