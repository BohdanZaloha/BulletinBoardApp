namespace BulletinBoardApi.Models
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) :base(message)
        {
            
        }
    }
}
