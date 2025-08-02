namespace BulletinBoardWeb.Services
{
    /// <summary>
    /// Thrown when the AnnouncementsService encounters an error.
    /// </summary>
    public class ServiceException : Exception
    {
        public ServiceException(string message, Exception innerException) :base(message, innerException) { }
    }
}
