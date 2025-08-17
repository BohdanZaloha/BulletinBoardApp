
using BulletinBoardApi.Data;
using BulletinBoardApi.Logging;
using BulletinBoardApi.MiddleWare;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BulletinBoardApi
{
    public class Program
    {
        public static int Main(string[] args)
        {

            LogBootstrapper.CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.AddSerilogLogging();
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
                builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
                builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
                // Add services to the container.

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();
                Log.Information("Current working directory: {WorkingDir}", Directory.GetCurrentDirectory());
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseMiddleware<ExceptionHandlingMiddleware>();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
