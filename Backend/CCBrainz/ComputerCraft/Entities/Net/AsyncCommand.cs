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

        public async Task<CompletionResult<T>> GetResult<T>(int timeout = 2000) 
        {
            var resultTask = _resultSource.Task;

            try
            {
                if (await Task.WhenAny(resultTask, Task.Delay(timeout)) == resultTask)
                {
                    var result = (resultTask.Result as JToken).ToObject<T>();

                    return CompletionResult<T>.FromComplete(result);
                }
                else
                {
                    return CompletionResult<T>.Empty;
                }
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

    public class CompletionResult<T>
    {
        public bool Completed { get; }

        public T Value 
        {
            get
            {
                if (!Completed)
                    throw new ArgumentNullException("Value is null!");
                else return _value;
            }
        }

        private T _value { get; }

        public CompletionResult(bool completed)
        {
            this.Completed = completed;
        }

        public CompletionResult(bool completed, T value)
        {
            this.Completed = completed;
            this._value = value;
        }


        internal static CompletionResult<T> FromComplete(T Value)
            => new CompletionResult<T>(true, Value);

        internal static CompletionResult<T> Empty
            => new CompletionResult<T>(false);
    }
}
