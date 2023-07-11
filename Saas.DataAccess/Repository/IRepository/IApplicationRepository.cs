using System.Linq.Expressions;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IApplicationRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
