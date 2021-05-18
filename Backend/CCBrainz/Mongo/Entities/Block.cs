using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.Mongo.Entities
{
    [BsonIgnoreExtraElements]
    public class Block
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public string Name { get; set; }

        public Dictionary<string, object> Tags { get; set; }

        public DateTime DateDiscovered { get; set; }

        public int DiscoveredBy { get; set; }
    }
}
