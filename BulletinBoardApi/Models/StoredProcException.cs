namespace BulletinBoardApi.Models
{
    public class StoredProcException : Exception
    {
        public int ErrorNumber { get; }

        public StoredProcException(int errorNumber, string message, Exception inner) : base(message, inner)
        {
            ErrorNumber = errorNumber;
        }

    }
}
