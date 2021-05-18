using CCBrainz.ComputerCraft.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCBrainz.ComputerCraft.Entities.Inventory
{
    public class Inventory
    {
        public IReadOnlyCollection<Item> Items 
        { 
            get
            {
                return _items;
            }
         }

        public int Size { get; }

        internal Turtle Owner { get; }

        private List<Item> _items { get; }

        public Inventory(int size, Turtle owner)
        {
            _items = new List<Item>(size);
            Size = size;
            EmptyItems();
            Owner = owner;
        }

        private void EmptyItems()
        {
            _items.Clear();
            for (int i = 0; i != Size; i++)
                _items.Add(null);
        }

        internal void Update(InventoryUpdated d)
        {
            if(d.Payload == null)
            {
                EmptyItems();
                return;
            }

            for(int i = 0; i != Size; i++)
            {
                var item = _items[i];
                var itemModel = d.Payload.FirstOrDefault(x => x.Key == i + 1).Value;

                if (item == null && itemModel == null)
                    continue;

                if (itemModel == null)
                    _items[i] = null;
                else if (item == null)
                    _items[i] = new Item(itemModel.Name, itemModel.Count, i + 1, this);
                else item.Update(itemModel.Name, itemModel.Count, i + 1);
            }
        }
    }
}
