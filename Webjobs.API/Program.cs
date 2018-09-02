using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Webjobs.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseKestrel()
				.UseIISIntegration()
				.UseStartup<Startup>();
				//.UseApplicationInsights();
	}
}
