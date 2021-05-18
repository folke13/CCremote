using CCBrainz.ComputerCraft.API;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.Mongo.Entities
{
    [BsonIgnoreExtraElements]
    public class TurtleData
    {
        public int Id { get; set; }
        public string Label { get; set; }

        public Location LastPosition { get; set; }

        public static TurtleData Get(int id)
            => MongoConnection.Overworld.Turtles.FirstOrDefault(x => x.Id == id);
    }
}
