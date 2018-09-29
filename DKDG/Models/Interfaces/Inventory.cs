using System;
using System.Collections.Generic;

using System.Runtime.Serialization;

using DKDG.Utils;

namespace DKDG.Models
{
    [DataContract, SQLSavableObject]
    public class Inventory
    {
        #region Fields

        private Dictionary<string, IItem> Items = new Dictionary<string, IItem>();

        #endregion Fields

        #region Properties

        public int Capacity { get; private set; }

        public double CurrentWeight { get; private set; }

        #endregion Properties

        #region Methods

        public void AddItem(IItem item)
        {
            if (CurrentWeight + item.Weight > Capacity)
                return; //TODO Display a message here somehow? show messagebox maybe? throw error maybe?

            Items.Add(item.GUID, item);
            CurrentWeight += item.Weight;
        }

        public void RemoveItem(string GUID)
        {
            if (Items.TryGetValue(GUID, out IItem item))
                RemoveItem(item);
            else
                throw new InvalidOperationException("Attempted to remove an item that does not exist in this inventory");
        }

        public void RemoveItem(IItem item)
        {
            CurrentWeight -= item.Weight;
            Items.Remove(item.GUID);
        }

        #endregion Methods
    }
}
