using System.Net;

namespace BulletinBoardWeb.Services
{
    /// <summary>
    /// Thrown when the AnnouncementsService encounters an error.
    /// </summary>
    public class ServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public ServiceException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
