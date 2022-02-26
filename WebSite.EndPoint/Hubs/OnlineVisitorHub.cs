using Application.Visitors.VisitorOnline;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebSite.EndPoint.Hubs
{
    public class OnlineVisitorHub:Hub
    {
        private readonly IVisitorOnlineService visitorOnline;
       
        public OnlineVisitorHub(IVisitorOnlineService visitorOnline)
        {
            this.visitorOnline = visitorOnline;
           
        }

        public override Task OnConnectedAsync()
        {
            string VisitorId = Context.GetHttpContext().Request.Cookies["VisitorId"];
            visitorOnline.ConnectUser(VisitorId);
            return base.OnConnectedAsync();
            visitorOnline.GetCount();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string VisitorId = Context.GetHttpContext().Request.Cookies["VisitorId"];
            visitorOnline.DisConnectUser(VisitorId);
            var count = visitorOnline.GetCount();
            return base.OnDisconnectedAsync(exception);
        }
    }
}
