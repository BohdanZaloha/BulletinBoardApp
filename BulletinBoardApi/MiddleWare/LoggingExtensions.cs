using Serilog;

namespace BulletinBoardApi.MiddleWare
{
    public static class Serilog
    {
        public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, services, lc) => lc
                .ReadFrom.Configuration(ctx.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());

            return builder;
        }
    }
}
