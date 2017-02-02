using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace GeneticImages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ImageSharp.Configuration.Default.AddImageFormat(new ImageSharp.Formats.PngFormat());

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
