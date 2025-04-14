using EntityFrameworkCore.UnitOfWork.Extensions;
using Microsoft.EntityFrameworkCore;
using udvSummerSchoolTestTask.DataBases;
using Serilog;
using udvSummerSchoolTestTask.Interfaces;
using udvSummerSchoolTestTask.Services;
using udvSummerSchoolTestTask.Repositories;

namespace udvSummerSchoolTestTask;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration).CreateLogger();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddHttpClient<IVkService, VkService>();
        services.AddScoped<IStatisticsRepository, StatisticsRepository>();
        
        builder.Services.AddUnitOfWork();
        builder.Services.AddUnitOfWork<ApplicationDbContext>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}