using CCBrainz.ComputerCraft;
using CCBrainz.Http.Websocket;
using CCBrainz.Mongo;
using CCBrainz.Mongo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.Mining
{
    public class MineLogic
    {
        public List<Turtle> ConnectedTurtles = new List<Turtle>();

        private List<Task> Routines = new List<Task>();

        private Turtle TunnelerOne;
        private Turtle TunnelerTwo;

        private List<Turtle> Miners;

        private WebSocketServer _server;
        private MongoConnection Database;

        private World World;

        public MineLogic(WebSocketServer server, MongoConnection conn)
        {
            _server = server;
            Database = conn;
            _server.OnTurtleConnected += _server_OnTurtleConnected;
        }

        private async Task _server_OnTurtleConnected(Turtle arg)
        {
            ConnectedTurtles.Add(arg);

            Console.WriteLine($"Turtle Connected! Id: {arg.ComputerId}, Total turtles: {ConnectedTurtles.Count}");
        }

        public void StartMining()
        {
            if (ConnectedTurtles.Count < 4)
                throw new Exception("Not enough turtles");

            TunnelerOne = ConnectedTurtles[0];
            TunnelerTwo = ConnectedTurtles[1];

            Miners = ConnectedTurtles.Skip(2).ToList();

            AddRoutine(Task.Run(async () =>
            {
                await TunnellerRoutine(TunnelerOne, TunnelerTwo);
            }));

            foreach(var miner in Miners)
            {
                AddRoutine(Task.Run(async () =>
                {
                    await MinerRoutine(miner);
                }));
            }
        }

        private void AddRoutine(Task t) 
        {
            Routines.Add(t);
        }

        public async Task TunnellerRoutine(Turtle one, Turtle two)
        {

        }

        public async Task MinerRoutine(Turtle turle)
        {

        }

        private void StopMining()
        {

        }
    }
}
