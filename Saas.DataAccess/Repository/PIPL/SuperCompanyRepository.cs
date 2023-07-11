using Microsoft.EntityFrameworkCore;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using System.Linq.Expressions;

namespace SaaS.DataAccess.Repository.PIPL
{
    public class SuperCompanyRepository<T> : ISuperCompanyRepository<T> where T : class
    {
        private readonly PIPLDeveloppementDbContext? context;
        internal DbSet<T> dbSet;

        public SuperCompanyRepository(PIPLDeveloppementDbContext context)
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
