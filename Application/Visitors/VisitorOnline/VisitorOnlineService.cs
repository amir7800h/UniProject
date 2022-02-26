using Application.Interfaces.Contexts;
using Domain.Visitors;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Visitors.VisitorOnline
{
    public class VisitorOnlineService : IVisitorOnlineService
    {
        private readonly IMongoDbContext<OnlineVisitor> mongoDbContext;
        private readonly IMongoCollection<OnlineVisitor> mongoCollection;
        public VisitorOnlineService(IMongoDbContext<OnlineVisitor> mongoDbContext)
        {
            this.mongoDbContext = mongoDbContext;
            mongoCollection = this.mongoDbContext.GetCollection();
        }
        public void ConnectUser(string ClientId)
        {
            if(!mongoCollection.AsQueryable().Any(p=> p.ClientId == ClientId))
            {
                mongoCollection.InsertOne(new OnlineVisitor
                {
                    ClientId = ClientId,
                    Time = DateTime.Now,
                });
            }
          
        }
        public void DisConnectUser(string ClientId)
        {
            mongoCollection.FindOneAndDelete(p => p.ClientId == ClientId);
        }

        public int GetCount()
        {
            return mongoCollection.AsQueryable().Count();
        }
    }
}
