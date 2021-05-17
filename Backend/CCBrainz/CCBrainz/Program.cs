using CCBrainz.Http.Websocket;
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


            await Task.Delay(-1);
        }
    }
}
