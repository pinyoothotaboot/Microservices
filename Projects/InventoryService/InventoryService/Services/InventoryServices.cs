using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryService.Models;
using InventoryService.Services;
using InventoryService.Databases;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace InventoryService.Services
{
    public class InventoryServices : IInventoryServices
    {
        private readonly Dictionary<string, InventoryItems> _inventoryItems;

        private MongoDatabases _mongo;

        public InventoryServices()
        {
            _inventoryItems = new Dictionary<string, InventoryItems>();

            _mongo = new MongoDatabases("mongodb://localhost:27017");
        }

        public async Task<InventoryItems> AddInventoryItemsAsync(InventoryItems items)
        {
         
            if (_mongo.isConnected()==true)
            {
                await _mongo.InsertOneInventory(items);

                return items;
            }

            return null;

        }

        public async Task<List<InventoryItems>> GetInventoryItemsAsync()
        {
            if (_mongo.isConnected() == true)
            {
                return await _mongo.GetAllInventories();
            }

            return null;

        }
    }
}
