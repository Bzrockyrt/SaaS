namespace SaaS.ViewModels.Application.GlobalWorkhour
{
    public class IndexGlobalWorkhourViewModel
    {
        public string Id { get; set; } = string.Empty;
        public DateTime WorkDay { get; set; }
        public TimeSpan MorningStart { get; set; }
        public TimeSpan MorningEnd { get; set; }
        public TimeSpan EveningStart { get; set; }
        public TimeSpan EveningEnd { get; set; }
        public bool Lunchbox { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
