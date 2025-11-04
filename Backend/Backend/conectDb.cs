using Microsoft.EntityFrameworkCore;
using Npgsql;
using Backend;
public class ConectDb
{
    public ConectDb(Microsoft.AspNetCore.Builder.WebApplicationBuilder? builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
             options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbContext"));
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });
    }
}

