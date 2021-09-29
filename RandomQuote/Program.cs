using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace RandomQuote
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder config)
        {
            IConfigurationRoot build = config.Build();

            if (context.HostingEnvironment.IsDevelopment())
            {
                
            }
            else if (context.HostingEnvironment.IsProduction())
            {
                config.AddAzureKeyVault(new SecretClient(new Uri(build["KeyVaultUrl"]), new DefaultAzureCredential()), new KeyVaultSecretManager());
            }
            
        }
    }
}
