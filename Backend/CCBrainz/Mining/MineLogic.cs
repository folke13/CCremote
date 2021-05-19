using CCBrainz.ComputerCraft;
using CCBrainz.ComputerCraft.API;
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
        private Turtle TunnelerThree;
        private Turtle TunnelerFour;

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
            if (ConnectedTurtles.Count < 7)
                throw new Exception("Not enough turtles");

            TunnelerOne = ConnectedTurtles[0];
            TunnelerTwo = ConnectedTurtles[1];
            TunnelerThree = ConnectedTurtles[2];
            TunnelerFour = ConnectedTurtles[3];


            Miners = ConnectedTurtles.Skip(4).ToList();

            AddRoutine(Task.Run(async () =>
            {
                await TunnellerRoutine(TunnelerOne, TunnelerTwo, TunnelerThree, TunnelerFour);
            }));

            foreach(var miner in Miners)
            {
                AddRoutine(Task.Run(async () =>
                {
                    await MinerRoutine(miner);
                }));
            }
        }

        /// <summary>
        ///     Mines a square. call with the turtle facing the bottom left block
        /// </summary>
        /// <param name="turtle">The turtle to control</param>
        /// <param name="size">The size of the square to mine</param>
        public static async Task MineSection(Turtle turtle, int size)
        {
            var result = await turtle.SendBatchCommandsAsync<List<BasicCommandResult>>(
                (CCOpCode.Dig, RelativeDirection.Forward),
                (CCOpCode.Move, Direction.Forward),
                (CCOpCode.Move, Direction.Left)
            );

            ValidateBatchResult(result);

            bool left = true;

            for (int y = 0; y != size - 1; y++)
            {
                for (int x = 0; x != size - 1; x++)
                {
                    var xMineResult = await turtle.SendBatchCommandsAsync<List<BasicCommandResult>>(
                        (CCOpCode.Dig, RelativeDirection.Forward),
                        (CCOpCode.Move, Direction.Forward)
                    );

                    ValidateBatchResult(xMineResult);
                }

                List<BasicCommandResult> ySwitchResult = await turtle.SendBatchCommandsAsync<List<BasicCommandResult>>(
                        (CCOpCode.Dig, RelativeDirection.Up),
                        (CCOpCode.Move, Direction.Up),
                        (CCOpCode.Move, Direction.Left),
                        (CCOpCode.Move, Direction.Left)
                    );

                ValidateBatchResult(ySwitchResult);

                left = !left;
            }
        }

        private static void ValidateBatchResult(List<BasicCommandResult> result)
        {
            if (result.Any(x => !x.Success))
            {
                throw new Exception($"Failed to execute batch command:\n{string.Join("\n", result.Select(x => $"Success: {x.Success} - Error: {x.Error}"))}");
            }
        }

        private void AddRoutine(Task t) 
        {
            Routines.Add(t);
        }

        public async Task TunnellerRoutine(Turtle one, Turtle two, Turtle three, Turtle four)
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
