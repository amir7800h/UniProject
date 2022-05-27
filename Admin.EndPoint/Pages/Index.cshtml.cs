using Application.Visitors.GetVisitorReports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.EndPoint.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IGetVisitorReportService getVisitorReportService;
        public ResultVisitorReportsDto ResultVisitorReports;
        public IndexModel(ILogger<IndexModel> logger, IGetVisitorReportService getVisitorReportService)
        {
            _logger = logger;
            this.getVisitorReportService = getVisitorReportService;
        }

        public void OnGet()
        {
            ResultVisitorReports = getVisitorReportService.Execute();
        }
    }
}