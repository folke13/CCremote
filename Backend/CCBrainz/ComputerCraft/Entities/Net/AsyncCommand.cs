using CCBrainz.Websocket.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.ComputerCraft
{
    internal class AsyncCommand
    {
        public CCOpCode OpCode { get; }
        public object Payload { get; }
        public string CommandNonse { get; }

        private TaskCompletionSource<object> _resultSource;

        public AsyncCommand(CCOpCode code, object payload)
        {
            _resultSource = new TaskCompletionSource<object>();
            this.OpCode = code;
            this.Payload = payload;

            this.CommandNonse = BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", "");
        }

        public void SetResult(object obj)
            => _resultSource.SetResult(obj);

        public async Task<T> GetResult<T>() 
        {
            var result = await _resultSource.Task;

            try
            {
                return (result as JToken).ToObject<T>();
            }
            catch(Exception x)
            {
                Console.Error.WriteLine(x);
                throw;
            }
        }

        internal SocketFrame ToSocketFrame()
        {
            return new SocketFrame()
            {
                OpCode = ReservedOpCodes.Command,
                Payload = new
                {
                    nonse = this.CommandNonse,
                    cmd = this.OpCode,
                    data = this.Payload
                }
            };
        }
    }
}
