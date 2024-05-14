using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using WebApplication1.Data;
using WebApplication1.Seeding;
using Serilog;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connection = builder.Configuration.GetConnectionString("MyTestDb");
        try
        {
            Log.Information("Starting web application");

            builder.Services.AddControllers();
            builder.Services.AddSerilog();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<MainContext>(options => options.UseInMemoryDatabase(connection));
            builder.Services.AddTransient<SeedDb>();

            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .WriteTo.File($"logs/log-.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();

            var app = builder.Build();
      
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using (var scope = app.Services.CreateScope())
                {
                    var seed = scope.ServiceProvider.GetRequiredService<SeedDb>();
                    seed.Run();
                }
            }
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        } 
        catch(Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }



}