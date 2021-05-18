using CCBrainz.Mongo.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.Mongo
{
    public class MongoConnection
    {
        public static MongoClient Client { get; private set; }

        public static IMongoDatabase Database
            => Client.GetDatabase("CCBrainz");

        public static IMongoCollection<World> WorldCollection
            => Database.GetCollection<World>("worlds");

        public static World Overworld
            => WorldCollection.Find(x => x.Dimension == "overworld").FirstOrDefault();

        public MongoConnection(string conn)
        {
            Client = new MongoClient(conn);

        }
    }
}
