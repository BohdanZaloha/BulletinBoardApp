using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EntityNotFoundException nfEx)
            {
                _log.LogWarning(nfEx, nfEx.Message);
                await WriteProblemDetailsResponseAsync(
                    context,
                    StatusCodes.Status404NotFound,
                    "https://bulletinboard.com/errors/not-found",
                    "Resource Not Found",
                    nfEx.Message);
            }
            catch (StoredProcException dbEx)
            {
                _log.LogError(dbEx, "Stored procedure error ({ErrorNumber})",
                    dbEx.ErrorNumber);
                await WriteProblemDetailsResponseAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    "https://bulletinboard.com/errors/database",
                    "Database Error",
                    dbEx.Message,
                    dbEx.ErrorNumber);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _log.LogWarning(uaEx, "Unauthorized access.");
                await WriteProblemDetailsResponseAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    "https://bulletinboard.com/errors/unauthorized",
                    "Unauthorized",
                    uaEx.Message);
            }
            catch (ArgumentException argEx)
            {
                _log.LogWarning(argEx, "Invalid argument.");
                await WriteProblemDetailsResponseAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    "https://bulletinboard.com/errors/invalid-argument",
                    "Invalid Argument",
                    argEx.Message);
            }
            catch (Exception ex)
            {
                _log.LogCritical(ex, "Unhandled exception.");
                await WriteProblemDetailsResponseAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "https://bulletinboard.com/errors/internal-server-error",
                    "Internal Server Error",
                    "An unexpected error occurred.");
            }
        }

        private async Task WriteProblemDetailsResponseAsync(
            HttpContext context,
            int statusCode,
            string type,
            string title,
            string detail,
            int? errorNumber = null)
        {
            var pd = new ProblemDetails
            {
                Type = type,
                Title = title,
                Status = statusCode,
                Detail = detail,
                Instance = context.Request.Path
            };

            pd.Extensions["traceId"] = context.TraceIdentifier;
            if (errorNumber.HasValue)
                pd.Extensions["errorNumber"] = errorNumber.Value;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = MediaTypeNames.Application.ProblemJson;
            await context.Response.WriteAsJsonAsync(pd);
        }
    }
}