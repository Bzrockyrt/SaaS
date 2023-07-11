namespace SaaS.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        void InitializePIPLDeveloppementDb();
        void InitializeApplicationDb();
    }
}
