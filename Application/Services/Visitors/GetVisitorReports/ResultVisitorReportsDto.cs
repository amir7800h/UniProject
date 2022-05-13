namespace Application.Visitors.GetVisitorReports
{
    public class ResultVisitorReportsDto
    {
        public GeneralStatsDto GeneralStats { get; set; }
        public TodayDto Today { get; set; }
        public List<VisitorsDto> Visitors { get; set; }
    }
}
