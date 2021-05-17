using CCBrainz.ComputerCraft.API;
using CCBrainz.Websocket.Net;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.ComputerCraft
{
    public class Turtle : BaseComputerCraftClient
    {
        public Turtle(HttpListenerWebSocketContext context, ComputercraftHello hello)
            : base(context, hello)
        {

        }

        #region Crafting

        public Task<CraftResult> Craft(int count)
            => base.SendCommandAsync<CraftResult>(CCOpCode.Craft, count);

        public Task<CraftResult> Craft()
            => base.SendCommandAsync<CraftResult>(CCOpCode.Craft);

        #endregion

        #region Detecting

        public Task<DetectResult> DetectForward()
          => Detect(RelativeDirection.Forward);

        public Task<DetectResult> DetectDown()
           => Detect(RelativeDirection.Down);

        public Task<DetectResult> DetectUp()
            => Detect(RelativeDirection.Up);

        public Task<DetectResult> Detect(RelativeDirection direction)
            => base.SendCommandAsync<DetectResult>(CCOpCode.Detect, direction);

        #endregion

        #region Sucking up items

        public Task<SuckResult> SuckForward()
          => Suck(RelativeDirection.Forward);

        public Task<SuckResult> SuckForward(int count)
            => Suck(RelativeDirection.Forward, count);

        public Task<SuckResult> SuckDown()
           => Suck(RelativeDirection.Down);

        public Task<SuckResult> SuckDown(int count)
            => Suck(RelativeDirection.Down, count);

        public Task<SuckResult> SuckUp()
           => Suck(RelativeDirection.Up);

        public Task<SuckResult> SuckUp(int count)
            => Suck(RelativeDirection.Up, count);

        public Task<SuckResult> Suck(RelativeDirection direction)
            => base.SendCommandAsync<SuckResult>(CCOpCode.Suck, direction);
        public Task<SuckResult> Suck(RelativeDirection direction, int count)
            => base.SendCommandAsync<SuckResult>(CCOpCode.Suck, new { direction = direction, count = count });

        #endregion 

        #region Dropping Items

        public Task<DropResult> DropForward()
          => Drop(RelativeDirection.Forward);

        public Task<DropResult> DropForward(int count)
            => Drop(RelativeDirection.Forward, count);

        public Task<DropResult> DropDown()
           => Drop(RelativeDirection.Down);

        public Task<DropResult> DropDown(int count)
            => Drop(RelativeDirection.Down, count);

        public Task<DropResult> DropUp()
           => Drop(RelativeDirection.Up);

        public Task<DropResult> DropUp(int count)
            => Drop(RelativeDirection.Up, count);

        public Task<DropResult> Drop(RelativeDirection direction)
            => base.SendCommandAsync<DropResult>(CCOpCode.Drop, direction);
        public Task<DropResult> Drop(RelativeDirection direction, int count)
            => base.SendCommandAsync<DropResult>(CCOpCode.Drop, new {direction = direction, count = count});

        #endregion

        #region Comparing

        public Task<CompareResult> CompareForward()
           => Compare(RelativeDirection.Forward);

        public Task<CompareResult> CompareDown()
           => Compare(RelativeDirection.Down);

        public Task<CompareResult> CompareUp()
            => Compare(RelativeDirection.Up);

        public Task<CompareResult> Compare(RelativeDirection direction)
            => base.SendCommandAsync<CompareResult>(CCOpCode.Compare, direction);

        public Task<CompareResult> CompareTo(int slot)
            => base.SendCommandAsync<CompareResult>(CCOpCode.CompareTo, slot);

        #endregion Comparing

        #region Placing

        public Task<PlaceResult> PlaceForward()
           => Place(RelativeDirection.Forward);

        public Task<PlaceResult> PlaceDown()
           => Place(RelativeDirection.Down);

        public Task<PlaceResult> PlaceUp()
            => Place(RelativeDirection.Up);

        public Task<PlaceResult> Place(RelativeDirection direction)
            => base.SendCommandAsync<PlaceResult>(CCOpCode.Place, direction);

        public Task<PlaceResult> PlaceSign(string text)
            => base.SendCommandAsync<PlaceResult>(CCOpCode.Place, text);

        #endregion Placing

        #region Digging
        public Task<DigResult> DigForward()
            => Dig(RelativeDirection.Forward);

        public Task<DigResult> DigDown()
            => Dig(RelativeDirection.Down);

        public Task<DigResult> DigUp()
            => Dig(RelativeDirection.Up);

        public Task<DigResult> Dig(RelativeDirection direction)
            => base.SendCommandAsync<DigResult>(CCOpCode.Dig, direction);
        #endregion Digging

        #region Attack
        public Task<AttackResult> AttackUp()
            => Attack(RelativeDirection.Up);

        public Task<AttackResult> AttackDown()
            => Attack(RelativeDirection.Down);

        public Task<AttackResult> AttackForward()
            => Attack(RelativeDirection.Forward);

        public Task<AttackResult> Attack(RelativeDirection direction)
            => SendCommandAsync<AttackResult>(CCOpCode.Attack, direction);

        #endregion Attack

        #region Inventory
        public Task<bool> Select(int slot)
            => base.SendCommandAsync<bool>(CCOpCode.Select, slot);

        public Task<int> GetSelectedSlot()
            => base.SendCommandAsync<int>(CCOpCode.GetSelectedSlot);

        public Task<int> GetItemCount()
            => base.SendCommandAsync<int>(CCOpCode.GetItemCount);
        public Task<int> GetItemCount(int slot)
            => base.SendCommandAsync<int>(CCOpCode.GetItemCount, slot);

        public Task<int> GetItemSpace()
            => base.SendCommandAsync<int>(CCOpCode.GetItemSpace);

        public Task<int> GetItemSpace(int slot)
            => base.SendCommandAsync<int>(CCOpCode.GetItemSpace, slot);

        public Task<ItemDetail> GetItemDetail()
            => base.SendCommandAsync<ItemDetail>(CCOpCode.GetItemDetail);

        public Task<ItemDetail> GetItemDetail(int slot)
            => base.SendCommandAsync<ItemDetail>(CCOpCode.GetItemDetail, slot);

        public Task<EquipResult> EquipLeft()
            => base.SendCommandAsync<EquipResult>(CCOpCode.Equip, Direction.Left);
        public Task<EquipResult> EquipRight()
            => base.SendCommandAsync<EquipResult>(CCOpCode.Equip, Direction.Right);

        public Task<TransferResult> TransferTo(int slot)
            => base.SendCommandAsync<TransferResult>(CCOpCode.TransferTo, slot);

        public Task<TransferResult> TransferTo(int slot, int count)
            => base.SendCommandAsync<TransferResult>(CCOpCode.TransferTo, new { slot = slot, count = count});

        #endregion Inventory

        #region Inspect
        public Task<InspectResult> InspectUp()
            => Inspect(RelativeDirection.Up);
        public Task<InspectResult> InspectDown()
            => Inspect(RelativeDirection.Down);
        public Task<InspectResult> InspectFoward()
            => Inspect(RelativeDirection.Forward);

        public Task<InspectResult> Inspect(RelativeDirection Direction)
        {
            return base.SendCommandAsync<InspectResult>(CCOpCode.Inspect, new
            {
                direction = Direction
            });
        }
        #endregion Inspect

        #region Movement
        public Task<MoveResult> MoveForward()
            => Move(Direction.Forward);
        public Task<MoveResult> MoveBack()
            => Move(Direction.Back);
        public Task<MoveResult> MoveRight()
            => Move(Direction.Right);
        public Task<MoveResult> MoveLeft()
            => Move(Direction.Left);
        public Task<MoveResult> MoveDown()
            => Move(Direction.Down);
        public Task<MoveResult> MoveUp()
            => Move(Direction.Up);

        public Task<MoveResult> Move(Direction direction)
        {
            return base.SendCommandAsync<MoveResult>(CCOpCode.Move, new
            {
                direction = direction
            });
        }
        #endregion Movement

        #region Fuel
        public Task<int> GetFuelLimit()
            => base.SendCommandAsync<int>(CCOpCode.GetFuelLimit, null);
        public Task<int> GetFuelLevel()
            => base.SendCommandAsync<int>(CCOpCode.GetFuelLevel, null);

        public Task<RefuelResult> Refuel()
            => base.SendCommandAsync<RefuelResult>(CCOpCode.Refuel, null);
        #endregion Fuel
    }

    public enum RelativeDirection
    {
        Up,
        Down,
        Forward
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Back
    }
}
