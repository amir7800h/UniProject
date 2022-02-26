using Application.Visitors.SaveVisitorInfo;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using UAParser;

namespace WebSite.EndPoint.Models.Utility.Filters
{
    public class SaveVisitorFilter : IActionFilter
    {
        private readonly ISaveVisitorInfoService _saveVisitorInfoService;
        public SaveVisitorFilter(ISaveVisitorInfoService saveVisitorInfoService)
        {
            _saveVisitorInfoService = saveVisitorInfoService;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string ip = context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var controllerName = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            var userAgent = context.HttpContext.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(userAgent);
            var referer = context.HttpContext.Request.Headers["Referer"].ToString();
            var currentUrl = context.HttpContext.Request.Path;
            string visitorId = context.HttpContext.Request.Cookies["VisitorId"];
            var request = context.HttpContext.Request;

            _saveVisitorInfoService.Execute(new RequestSaveVisitorInfoDto
            {
                Browser = new VisitorVersionDto
                {
                    Family = clientInfo.UA.Family,
                    Version = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}.{clientInfo.UA.Patch}",
                },
                Device = new DeviceDto
                {
                    Family =clientInfo.Device.Family,
                    Brand = clientInfo.Device.Brand,
                    IsSpider = clientInfo.Device.IsSpider,
                    Model = clientInfo.Device.Model,
                },
                OperationSystem = new VisitorVersionDto
                {
                    Family=clientInfo.OS.Family,
                    Version = $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}.{clientInfo.OS.Patch}.{clientInfo.OS.PatchMinor}"
                },
                CurrentLink = currentUrl,
                Ip = ip,
                PhysicalPath = $"{controllerName}/{actionName}",
                ReferrerLink = referer,
                Protocol = request.Protocol,
                Method = request.Method,
                VisitorId = visitorId,
            });
        }
    }
}
