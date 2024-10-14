using Microsoft.EntityFrameworkCore;
using SimpleTwitter.Api.Infrastructure;

namespace SimpleTwitter.Api.Extensions;

public static class WebAppExtensions
{
    public static void InitializeDb(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }
        
        using var scope = app.Services.CreateScope();
        {
            scope.ServiceProvider.GetRequiredService<SimpleTwitterDbContext>().Database.Migrate();
        }
    }
}