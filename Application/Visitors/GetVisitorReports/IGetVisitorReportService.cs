using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Visitors.GetVisitorReports
{
    public interface IGetVisitorReportService
    {
        ResultVisitorReportsDto Execute();
    }
}
