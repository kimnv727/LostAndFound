namespace LostAndFound.Infrastructure.Repositories.Interfaces.Common
{
    public interface IDelete<T> where T : class
    {
        void Delete(T obj);
    }
}
