using Application.Interfaces.Contexts;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts.MongoContext
{
    public class MongoDbContext<T> : IMongoDbContext<T>
    {
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<T> mongoCollection;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient();
            db = client.GetDatabase(configuration["MongoDb:VisitorDataBaseName"]);
            mongoCollection = db.GetCollection<T>(typeof(T).Name);
        }
        public IMongoCollection<T> GetCollection()
        {
            return mongoCollection;
        }
    }
}
