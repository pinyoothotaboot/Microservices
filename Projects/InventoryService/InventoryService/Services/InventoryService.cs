using System;
using System.Collections.Generic;
using InventoryService.Models;
using InventoryService.Services;

namespace InventoryService.Services
{
    public class InventoryService : IInventoryServices
    {

        readonly Dictionary<string, InventoryItems> _inventoryItems;

        public InventoryService()
        {
            _inventoryItems = new Dictionary<string, InventoryItems>();
        }

        public InventoryItems AddInventoryItems(InventoryItems items)
        {
            _inventoryItems.Add(items.ItemName, items);

            return items;
        }

        public Dictionary<string, InventoryItems> GetInventoryItems()
        {
            return _inventoryItems;
        }
    }
}
