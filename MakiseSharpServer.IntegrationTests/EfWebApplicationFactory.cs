using MakiseSharpServer.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MakiseSharpServer.IntegrationTests
{
    public class EfWebApplicationFactory : WebApplicationFactory<API.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory
                // database for testing.
                services.AddDbContext<MakiseDbContext>(options =>
                {
                    options.UseInMemoryDatabase("MakiseSharpServerFunctionalTestingDb");
                    options.UseInternalServiceProvider(serviceProvider);
                });
                services.AddDbContext<KeysDbContext>(options =>
                {
                    options.UseInMemoryDatabase("MakiseSharpServerKeysContextDb");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                //Setup configuration
                sp.GetService<IConfiguration>().SetupTestConfiguration();

                // Create a scope to obtain a reference to the database context
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MakiseDbContext>();
                    var db2 = scopedServices.GetRequiredService<KeysDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<EfWebApplicationFactory>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();
                    db2.Database.EnsureCreated();

                    /*try
                    {
                        // Seed the database with test data.
                        Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex,
                            "An error occurred seeding the database with test messages. Error: {ex.Message}");
                    }*/
                }
            });
        }
    }
}
