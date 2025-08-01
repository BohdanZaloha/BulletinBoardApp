using BulletinBoardWeb.Models;
using BulletinBoardWeb.Services;
using System.Net.Http.Headers;

namespace BulletinBoardWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient<AnnouncementsService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });   

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Announcements}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
