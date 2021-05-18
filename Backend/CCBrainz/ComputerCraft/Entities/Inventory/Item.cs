using CCBrainz.ComputerCraft.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.ComputerCraft.Entities.Inventory
{
    public class Item
    {
        public string Name { get; internal set; }
        public int Count { get; internal set; }
        public int Location { get; internal set; }

        private Inventory Parent { get; }

        internal Item(string name, int count, int location, Inventory parent)
        {
            this.Name = name;
            this.Count = count;
            this.Location = location;
            this.Parent = parent;
        }

        internal void Update(string name, int count, int location)
        {
            this.Name = name;
            this.Count = count;
            this.Location = location;
        }

        public override string ToString()
        {
            return $"{this.Location} - {this.Name}:{this.Count}";
        }

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
            => Parent.Owner.Drop(direction);

        public Task<DropResult> Drop(RelativeDirection direction, int count)
            => Parent.Owner.Drop(direction, count);
    }
}
