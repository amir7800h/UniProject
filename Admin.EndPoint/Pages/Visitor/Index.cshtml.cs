using Application.Visitors.GetVisitorReports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.EndPoint.Pages.Visitor
{
    public class IndexModel : PageModel
    {
        private IGetVisitorReportService getVisitorReportService;
        public ResultVisitorReportsDto ResultVisitorReports;
        public IndexModel(IGetVisitorReportService getVisitorReportService)
        {
            this.getVisitorReportService = getVisitorReportService;
        }
        public void OnGet()
        {
            ResultVisitorReports = getVisitorReportService.Execute();
        }
    }
}
