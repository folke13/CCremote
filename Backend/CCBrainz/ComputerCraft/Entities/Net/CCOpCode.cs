using System;
using System.Collections.Generic;
using System.Text;

namespace CCBrainz.ComputerCraft
{
    public enum CCOpCode
    {
        #region Reserved
        Hello = 0,
        HelloAccepted = 1,
        Eval = 2,
        EvalResponse = 3,
        Command = 4,
        CommandResponse = 5,
        BatchCommand = 6,
        BatchCommandResult = 7,
        Event = 8,
        #endregion

        #region Turtle
        Move = 12,
        Select = 13,
        GetSelectedSlot = 14,
        GetItemCount = 15,
        GetItemSpace = 16,
        GetItemDetail = 17,
        Equip = 18,
        Attack = 19,
        Dig = 20,
        Place = 21,
        Detect = 22,
        Inspect = 23,
        Compare = 24,
        CompareTo = 25,
        Drop = 26,
        Suck = 27,
        Refuel = 28,
        GetFuelLevel = 29,
        GetFuelLimit = 30,
        TransferTo = 31,
        Craft = 32,
        GPSLocate = 33,
        #endregion Turtle
    }
}
