using AutoScrum.Infrastructure.Blazor;
using AutoScrum.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoScrum
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
	        var builder = WebAssemblyHostBuilder.CreateDefault(args);
	        builder.RootComponents.Add<App>("#app");
	        builder.RootComponents.Add<HeadOutlet>("head::after");

	        var services = builder.Services;

	        services.AddScoped(_ => new HttpClient
	        {
		        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
	        });

	        services.AddTransient<OldConfigService>();
			services.AddBlazorInfrastructure();
	        services.AddTransient<AutoMessageService>();

	        services.AddAntDesign();

	        var host = builder.Build();

	        await host.RunAsync();

        }
    }
}
