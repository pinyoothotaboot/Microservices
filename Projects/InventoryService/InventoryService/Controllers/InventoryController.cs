using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryService.Models;
using InventoryService.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace InventoryService.Controllers
{
    [Route("api/v1/inventories")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly  IInventoryServices _services;

        public InventoryController(IInventoryServices services)
        {
            _services = services;
        }

        // GET api/v1/inventories
        [HttpGet]
        public async Task<ActionResult<BsonDocument>> GetAsync()
        {
            var inventoryItems = _services.GetInventoryItemsAsync();

            if (inventoryItems == null)
            {
                return NotFound();
            }

            Task<List<BsonDocument>> inventoryItems1 = inventoryItems;

            var items =  await inventoryItems1;

            var root = new BsonDocument() {
                {
                    new BsonDocument()
                }
            };

            foreach (var item in items)
            {
                root[item[0].ToString()] = new BsonDocument()
                    {
                        {"Id",item[0].ToString()},
                        {"Version",item[1].ToString()},
                        {"Create_Date",item[2].ToString()},
                        {"Start_Date",item[3].ToString()},
                        {"End_Date",item[4].ToString()},
                        {"Display_Name",item[5].ToString()},
                        {"Description",item[6].ToString()},
                        {"Catalog_Ref_Id",item[7].ToString()},
                        {"Availability_Status",item[8].ToString()},
                        {"Availability_Date",item[9].ToString()},
                        {"Stock_Level",item[10].ToString()},
                        {"Stock_Thresh",item[11].ToString()},
                        {"Product_Id",item[12].ToString()},
                        {"Created_By",item[13].ToString()}
                    };
            }
            return root;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<BsonDocument>> GetAsync(string id)
        {
            var root = new BsonDocument() { 
                {"results",new BsonDocument() } };

            if (id == null)
            {
                return NotFound();
            }

           

            root["results"][id] = new BsonDocument() { 
                {"id",id.ToString()}
            };

            return root;
           
        }

        // api/v1/inventories
        [HttpPost]
        public async Task<ActionResult<InventoryItems>> PostAsync(InventoryItems items)
        {
            var inventoryItems =  _services.AddInventoryItemsAsync(items);

            if (inventoryItems == null)
            {
                return NotFound();
            }

            return await inventoryItems;
        }

        // PUT api/v1/inventories/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/v1/inventories/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
