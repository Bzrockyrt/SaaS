using Microsoft.EntityFrameworkCore;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;
using System.Linq.Expressions;

namespace SaaS.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //private readonly ApplicationDbContext context;
        private readonly ApplicationDbContext context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<T>();
        }

        public void Add(T entity)
        {
            this.dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            this.dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            this.dbSet.RemoveRange(entities);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = this.dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = this.dbSet;
            return query.ToList();
        }
    }
}
