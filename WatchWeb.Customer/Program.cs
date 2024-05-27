using WatchWeb.Admin;

public class Program
{

    public static Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                _ = webHostBuilder
                    .UseStartup<Startup>();
            })
            .Build();
        return host.RunAsync();
    }
}