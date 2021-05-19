using CCBrainz.ComputerCraft;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.Mongo.Entities
{
    [BsonIgnoreExtraElements]
    public class World
    {
        public string Dimension { get; set; }

        public List<Block> Blocks { get; set; }

        public List<TurtleData> Turtles { get; set; }

        public void CreateTurtle(Turtle turtle)
        {
            var turtleData = new TurtleData()
            {
                Id = turtle.ComputerId,
                //Label = turtle.lab
            };
        }
    }
}
