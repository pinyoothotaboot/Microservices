using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryService.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace InventoryService.Services
{
    public interface IInventoryServices
    {
        Task<InventoryItems> AddInventoryItemsAsync(InventoryItems items);

        Task<List<BsonDocument>> GetInventoryItemsAsync();
        
    }
}
