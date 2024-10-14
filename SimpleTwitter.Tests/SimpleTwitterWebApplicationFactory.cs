using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleTwitter.Api.Infrastructure;

namespace SimpleTwitter.Tests;

public class SimpleTwitterWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<SimpleTwitterDbContext>));
            services.Remove(dbContextDescriptor);
            var dbContext = services.SingleOrDefault(
                d => d.ServiceType == typeof(SimpleTwitterDbContext));
            services.Remove(dbContext);

            services.AddDbContext<SimpleTwitterDbContext>(opt =>
            {
                opt.UseInMemoryDatabase("simple-twitter");
            });
        });

        builder.UseEnvironment("Testing");
    }
}