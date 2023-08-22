namespace LostAndFound.Infrastructure.Repositories.Interfaces.Common
{
    public interface IUpdate<T> where T : class
    {
        void Update(T obj);
    }
}
