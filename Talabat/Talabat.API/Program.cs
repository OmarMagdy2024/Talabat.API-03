
using Microsoft.EntityFrameworkCore;
using Talabat.API.Profiless;
using Talabat.API.Profiless;
using Talabat.Core.Interfaces;
using Talabat.Core.Models;
using Talabat.Repository.Connections;
using Talabat.Repository.DataSeeding;
using Talabat.Repository.Repositories;

namespace Talabat.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<TalabatDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaulfConnection")));
        //builder.Services.AddScoped<IGenaricRepository<Product>, GenaricRepository<Product>>();
        //builder.Services.AddScoped<IGenaricRepository<ProductBrand>, GenaricRepository<ProductBrand>>();
        //builder.Services.AddScoped<IGenaricRepository<ProductType>, GenaricRepository<ProductType>>();
        builder.Services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
        builder.Services.AddAutoMapper(typeof(Profiles));
        var app = builder.Build();

        //{
        //    var services = scope.ServiceProvider;
        //    var dbcontext = services.GetRequiredService<TalabatDBContext>();
        //    await dbcontext.Database.MigrateAsync();
        //}
        //finally
        //{
        //    scope.Dispose();
        //}
        using var scope = app.Services.CreateScope();
		var services = scope.ServiceProvider;
		var dbcontext = services.GetRequiredService<TalabatDBContext>();
        var loggerfactory=services.GetRequiredService<ILoggerFactory>();
        try
        {
            await dbcontext.Database.MigrateAsync();
            await TalabatContextSeed.SeedAsync(dbcontext);
        }
        catch (Exception ex)
        {
            var logger = loggerfactory.CreateLogger<Program>();
            logger.LogError(ex, "An Error Occurred During Migration");
        }        
		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.UseStaticFiles();

        app.Run();
    }
}
