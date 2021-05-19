using CCBrainz.ComputerCraft;
using CCBrainz.Http.Websocket;
using CCBrainz.Mining;
using System;
using System.Threading.Tasks;

namespace CCBrainz
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            var server = new WebSocketServer(3000);

            server.OnTurtleConnected += Server_OnTurtleConnected;

            await Task.Delay(-1);
        }

        private async Task Server_OnTurtleConnected(Turtle arg)
        {
            Console.WriteLine($"Turtle {arg.ComputerId} Connected!");
            arg.InventoryUpdated += Arg_InventoryUpdated;
        }

        private async Task Arg_InventoryUpdated(ComputerCraft.Entities.Inventory.Inventory arg)
        {
            await MineLogic.MineSection(arg.Owner, 6);
        }
    }
}
