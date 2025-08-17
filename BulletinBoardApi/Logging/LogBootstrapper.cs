using Serilog;

namespace BulletinBoardApi.Logging
{
    public static class LogBootstrapper
    {
        /// <summary>
        /// Lightweight logger so startup failures are visible.
        /// </summary>
        public static void CreateBootstrapLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console(new Serilog.Formatting.Compact.RenderedCompactJsonFormatter())
                .CreateBootstrapLogger();
        }
    }
}
