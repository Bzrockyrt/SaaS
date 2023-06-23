namespace SaaS.Domain.OTHER
{
    public class CompanySetting : ModelBase
    {
        public CompanySetting() : base()
        {
            if (string.IsNullOrEmpty(this.Id))
                Id = Guid.NewGuid().ToString();
        }
    }
}
