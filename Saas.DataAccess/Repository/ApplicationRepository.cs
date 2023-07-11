using Microsoft.EntityFrameworkCore;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace SaaS.DataAccess.Repository
{
    public class ApplicationRepository<T> : IApplicationRepository<T> where T : class
    {
        private readonly ApplicationDbContext? context;
        internal DbSet<T> dbSet;

        public ApplicationRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = this.context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }
    }
}
