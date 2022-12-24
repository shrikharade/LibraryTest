using BookLibrary.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTestCase.Setup
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<Program> where TProgram : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<BookContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddDbContext<BookContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryBookTest");
                });
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<BookContext>())
                {
                    try
                    {
                        appContext.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            });
        }
    }
}
