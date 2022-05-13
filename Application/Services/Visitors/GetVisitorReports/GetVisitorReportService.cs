using Application.Interfaces.Contexts;
using Domain.Visitors;
using MongoDB.Driver;

namespace Application.Visitors.GetVisitorReports
{
    public class GetVisitorReportService : IGetVisitorReportService
    {
        private readonly IMongoDbContext<Visitor> mongoDbContext;
        private readonly IMongoCollection<Visitor> visitorMongoCollection;
        public GetVisitorReportService(IMongoDbContext<Visitor> mongoDbContext)
        {
            this.mongoDbContext = mongoDbContext;
            visitorMongoCollection = mongoDbContext.GetCollection();
        }
        public ResultVisitorReportsDto Execute()
        {
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now.Date.AddDays(1);

            var TodayPageViewCount = visitorMongoCollection.AsQueryable()
                .Where(p=> p.Time >= start && p.Time <= end).LongCount();

            var TodayVistorCount = visitorMongoCollection.AsQueryable()
                .Where(p=> p.Time >= start && p.Time <= end)
                .GroupBy(p=> p.VisitorId).LongCount();

            var AllPageViewCount = visitorMongoCollection.AsQueryable().LongCount();

            var AllVisitorCount = visitorMongoCollection.AsQueryable()
                .GroupBy(p => p.VisitorId).LongCount();

            var TodayPageViewList = visitorMongoCollection.AsQueryable()
                .Where(p => p.Time >= start && p.Time <= end)
                .Select(p => new { p.Time }).ToList();

            VisitCountDto visitPerHour = GetVisitorPerHour(start, end);
            VisitCountDto visitPerDay = GetVisitPerDay();
            List<VisitorsDto> visitors = GettLastVisitor(10);

            return new ResultVisitorReportsDto()
            {
                GeneralStats = new GeneralStatsDto
                {
                    TotalPageViews = AllPageViewCount,
                    TotalVisitors = AllVisitorCount,
                    PageViewsPerVisit = GetAvg(AllVisitorCount, AllVisitorCount),
                    VisitPerDay = visitPerDay,
                },
                Today = new TodayDto
                {
                    PageViews = TodayPageViewCount,
                    Visitors = TodayVistorCount,
                    ViewsPerVisitor = GetAvg(TodayPageViewCount, TodayVistorCount),
                    VisitPerhour = visitPerHour,
                },
                Visitors = visitors,

            };
        }
        private VisitCountDto GetVisitorPerHour(DateTime start, DateTime end)
        {
            var TodayPageViewList = visitorMongoCollection.AsQueryable()
               .Where(p => p.Time >= start && p.Time <= end)
               .Select(p => new { p.Time }).ToList();
            VisitCountDto visitPerHour = new VisitCountDto()
            {
                Display = new string[24],
                Value = new int[24]
            };

            for (int i = 0; i <= 23; i++)
            {
                visitPerHour.Display[i] = $"{i}-h";
                visitPerHour.Value[i] = TodayPageViewList
                    .Where(p => p.Time.Hour == i).Count();
            }
            return visitPerHour;
        }

        private VisitCountDto GetVisitPerDay()
        {
            DateTime MonthStart = DateTime.Now.Date.AddDays(-30);
            DateTime MonthEnds = DateTime.Now.Date.AddDays(1);
            var Month_PageViewList = visitorMongoCollection.AsQueryable()
                .Where(p => p.Time >= MonthStart && p.Time < MonthEnds)
                .Select(p => new { p.Time })
                .ToList();
            VisitCountDto visitPerDay = new VisitCountDto() { Display = new string[31], Value = new int[31] };
            for (int i = 0; i <= 30; i++)
            {
                var currentday = DateTime.Now.AddDays(i * (-1));
                visitPerDay.Display[i] = i.ToString();
                visitPerDay.Value[i] = Month_PageViewList.Where(p => p.Time.Date == currentday.Date).Count();
            }
            visitPerDay.Display = visitPerDay.Display.Reverse().ToArray();
            visitPerDay.Value = visitPerDay.Value.Reverse().ToArray();
            return visitPerDay;
        }

        private List<VisitorsDto> GettLastVisitor(int Number)
        {
           List<VisitorsDto> visitors = visitorMongoCollection.AsQueryable()
                .OrderByDescending(p => p.Time)
                .Take(Number)
                .Select(p => new VisitorsDto
                {
                    Id = p.Id,
                    Browser = p.Browser.Family,
                    CurrentLink = p.CurrentLink,
                    Ip = p.Ip,
                    OperationSystem = p.OperationSystem.Family,
                    IsSpider = p.Device.IsSpider,
                    ReferrerLink = p.ReferrerLink,
                    Time = p.Time,
                    VisitorId = p.VisitorId
                }).ToList();
            return visitors;
        }
        private float GetAvg(long VisitPage, long Visitors)
        {
            if(Visitors == 0)
            {
                return 0;
            }
            else
            {
                return VisitPage / Visitors;
            }
        }
    }
}
