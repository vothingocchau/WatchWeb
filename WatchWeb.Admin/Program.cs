using WatchWeb.Admin;
using WatchWeb.Model;

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

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                _ = SeedData.Initialize(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        return host.RunAsync();
    }
}