using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulletinBoardApi.MiddleWare
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _log;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task InvokeAsync(HttpContext Ctx)
        {
            try
            {
                await _next(Ctx);
            }
            catch (StoredProcException dbEx)
            {
                _log.LogError(dbEx, "SQL SP Error {ErrorNumber}", dbEx.ErrorNumber);
                Ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                Ctx.Response.ContentType = "application/problem+json";
                var problem = new ProblemDetails
                {
                    Type = "https",
                    Title = "database error",
                    Status = 400,
                    Detail = dbEx.Message
                };
                await Ctx.Response.WriteAsJsonAsync(problem);
            }
            catch (Exception ex)
            {
                _log.LogCritical(ex, "unknown error on the server");
                Ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
                Ctx.Response.ContentType = "application/problem+json";
                var problem = new ProblemDetails
                {
                    Type = "https",
                    Title = "internal server error",
                    Status = 500

                };
                await Ctx.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
