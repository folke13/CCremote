using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.Websocket.Net
{
    public enum ReservedOpCodes
    {
        Hello = 0,
        HelloAccepted = 1,
        Eval = 2,
        EvalResponse = 3,
        Command = 4,
        CommandResponse = 5,
        BatchCommand = 6,
        BatchCommandResult = 7,
        Event = 8
    }
}
