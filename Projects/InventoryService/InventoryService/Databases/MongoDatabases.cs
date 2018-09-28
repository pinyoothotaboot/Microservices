using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryService.Models;

namespace InventoryService.Databases
{
    public class MongoDatabases
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<InventoryItems> _inventoryCollection;

        public MongoDatabases(string connection)
        {
            _client = new MongoClient(connection);
            _database = _client.GetDatabase("databases");
            _inventoryCollection = _database.GetCollection<InventoryItems>("inventories");
        }

        public bool isConnected()
        {
            try
            {
                _database.ListCollections();
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public async Task<List<BsonDocument>> GetAllInventories()
        {
            var projection = Builders<InventoryItems>.Projection
                                                     .Include("version")
                                                     .Include("creation_date")
                                                     .Include("start_date")
                                                     .Include("end_date")
                                                     .Include("display_name")
                                                     .Include("description")
                                                     .Include("catalog_ref_id")
                                                     .Include("availability_status")
                                                     .Include("availability_date")
                                                     .Include("stock_level")
                                                     .Include("stock_thresh")
                                                     .Include("product_id")
                                                     .Include("created_by");

            return await _inventoryCollection
                .Find(new BsonDocument())
                .Project<BsonDocument>(projection)
                .ToListAsync();
        }

        public async Task<List<InventoryItems>> GetInventoriesByField(string fieldName,string fieldValue)
        {
            var filter = Builders<InventoryItems>.Filter.Eq(fieldName, fieldValue);

            var result = await _inventoryCollection.Find(filter).ToListAsync();

            return result;
        }

        public async Task<List<InventoryItems>> GetInventories(int startFrom,int count)
        {
            var result = await _inventoryCollection
                .Find(new BsonDocument())
                .Skip(startFrom)
                .Limit(count)
                .ToListAsync();

            return result;
        }

        public async Task InsertOneInventory(InventoryItems items)
        {
            try
            {
                await _inventoryCollection.InsertOneAsync(items);
            }
            catch (MongoException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> DeleteInventoryById(ObjectId id)
        {
            var filter = Builders<InventoryItems>.Filter.Eq("_id", id);
            var result = await _inventoryCollection.DeleteOneAsync(filter);

            return result.DeletedCount != 0;
        }

        public async Task<long> DeleteAllInventories()
        {
            var filter = new BsonDocument();
            var result = await _inventoryCollection.DeleteManyAsync(filter);

            return result.DeletedCount;
        }

        public async Task<bool> UpdateInventory(ObjectId id,string updateFieldName,string updateFieldValue)
        {
            var check = true;

            try
            {
                var filter = Builders<InventoryItems>.Filter.Eq("_id", id);
                var update = Builders<InventoryItems>.Update.Set(updateFieldName, updateFieldValue);

                var result = await _inventoryCollection.UpdateOneAsync(filter, update);

                check = result.ModifiedCount != 0;

            }
            catch (MongoException ex)
            {
                throw new Exception(ex.Message);
            }

            return check;

        }

        public async Task CreateIndexOnNameField()
        {
            var keys = Builders<InventoryItems>.IndexKeys.Ascending(x => x.Display_Name);
            await _inventoryCollection.Indexes.CreateOneAsync(keys);
        }

        public async Task CreateIndexOnCollection(IMongoCollection<BsonDocument> collection, string field)
        {
            var keys = Builders<BsonDocument>.IndexKeys.Ascending(field);
            await collection.Indexes.CreateOneAsync(keys);
        }
    }
}
