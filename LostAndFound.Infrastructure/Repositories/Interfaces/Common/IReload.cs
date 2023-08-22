namespace LostAndFound.Infrastructure.Repositories.Interfaces.Common
{
    public interface IReload<T> where T : class
    {
        void Reload(T obj);
    }
}
