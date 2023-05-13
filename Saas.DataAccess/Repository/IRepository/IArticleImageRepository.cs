using SaaS.Domain.Models;

namespace SaaS.DataAccess.Repository.IRepository
{
    public interface IArticleImageRepository : IRepository<ArticleImage>
    {
        public void Update(ArticleImage articleImage);
    }
}
