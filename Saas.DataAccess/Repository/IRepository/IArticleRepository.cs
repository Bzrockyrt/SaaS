using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IArticleRepository : IRepository<Article>
    {
        public void Update(Article article);
    }
}
