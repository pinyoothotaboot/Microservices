using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryService.Models;
using InventoryService.Services;
using MongoDB.Bson;
using InventoryService.Security;

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
        public async Task<ActionResult<Dictionary<string,Dictionary<string,string>>>> GetAsync()
        {
            var inventoryItems = _services.GetInventoryItemsAsync();

            if (inventoryItems == null)
            {
                return NotFound();
            }

            Task<List<InventoryItems>> inventoryItems1 = inventoryItems;

            var items =  await inventoryItems1;

            Dictionary<string, Dictionary<string,string>> json = new Dictionary<string, Dictionary<string,string>>();

            foreach(var item in items)
            {
                json.Add(item.Id.ToString(),new Dictionary<string, string>(){
                    {"version",item.Version.ToString()},
                    {"createdDate",item.Creation_Date.ToString()},
                    {"startDate",item.Start_Date.ToString()},
                    {"endDate",item.End_Date.ToString()},
                    {"displayName",item.Display_Name.ToString()},
                    {"description",item.Description.ToString()},
                    {"catalogRefId",item.Catalog_Ref_Id.ToString()},
                    {"availabilityStatus",item.Availability_Status.ToString()},
                    {"availabilityDate",item.Availability_Date.ToString()},
                    {"stockLevel",item.Stock_Level.ToString()},
                    {"stockThresh",item.Stock_Thresh.ToString()},
                    {"productId",item.Product_Id.ToString()},
                    {"createdBy",item.Created_By.ToString()}
                });
            }

            return json;

        }

        [HttpGet("filter")]
        public async Task<ActionResult<BsonDocument>> GetAsync(string byId,string byVersion,string byDisplayname,
                                                               string byCreate_date,string byStart_date,
                                                               string byEnd_date,string byCatalog_Id,string byAvail_status,
                                                               string byStock_level,string byStock_thresh,string byCreate_id,
                                                               string sortBy,string limit,string start)
        {

            IDictionary<string, string> param = new Dictionary<string, string>();

            var root = new BsonDocument() 
            { 
                {"results",new BsonDocument() } 
            };

            if ((byId == null) && (byVersion == null) && (byDisplayname == null) &&
                (byCreate_date == null) && (byStart_date == null) && (byEnd_date == null) &&
                (byCatalog_Id == null) && (byAvail_status == null) && (byStock_level == null) &&
                (byStock_thresh == null) && (byCreate_id == null) && (sortBy == null) &&
                (limit == null) && (start == null))
            {
                return NotFound();
            }

            if (byId != null)
            {
                param.Add("id", byId.ToString());
            }

            if (byVersion != null)
            {
                param.Add("version", byVersion.ToString());
            }



            return root;
           
        }

        // api/v1/inventories
        [HttpPost]
        public async Task<ActionResult<InventoryItems>> PostAsync(Dictionary<string,object> items)
        {

           
            Dictionary<string, string> msg;

            var check = new ParameterInput();

            if (!string.IsNullOrWhiteSpace(items["Version"].ToString()) &&
                items["Version"].ToString().Trim().Length!=0)
            {
                if (check.isValidVersion(items["Version"].ToString()) !=true)
                {
                    msg = new Dictionary<string, string>()
                    { 
                        {"msg","Version incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Version can't empty."}
                    };
                return BadRequest(msg);
            } 

            if (!string.IsNullOrWhiteSpace(items["Start_Date"].ToString()) &&
                items["Start_Date"].ToString().Trim().Length != 0)
            {
                DateTime myDateTime = DateTime.SpecifyKind(
                   DateTime.Parse(items["Start_Date"].ToString()), 
                    DateTimeKind.Utc);
                string a = myDateTime.ToString("o");

                if (check.isValidStart_Date(a)!=true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Start date incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Start date can't empty."}
                    };
                return BadRequest(msg);
            }

            if (!string.IsNullOrWhiteSpace(items["End_Date"].ToString()) &&
                items["End_Date"].ToString().Trim().Length != 0)
            {
                DateTime myDateTime = DateTime.SpecifyKind(
                    DateTime.Parse(items["End_Date"].ToString()), 
                   DateTimeKind.Utc);
                string a = myDateTime.ToString("o");

                if (check.isValidEnd_Date(a) !=true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","End date incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","End date can't empty."}
                    };
                return BadRequest(msg);
            }

            if (!string.IsNullOrWhiteSpace(items["Display_Name"].ToString()) &&
                items["Display_Name"].ToString().Trim().Length != 0)
            {
                if (check.isValidDisplay_Name(items["Display_Name"].ToString()) !=true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Display name incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Display name can't empty."}
                    };
                return BadRequest(msg);
            }

            if (!string.IsNullOrWhiteSpace(items["Description"].ToString()) &&
                items["Description"].ToString().Trim().Length!=0)
            {
                if (check.isValidDescription(items["Description"].ToString()) != true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Description incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Description can't empty."}
                    };
                return BadRequest(msg);
            }

            if (!string.IsNullOrWhiteSpace(items["Catalog_Ref_Id"].ToString()))
            {
                if (items["Catalog_Ref_Id"].ToString().Trim().Length != 24)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Catalog ref id length is 24"}
                    };
                    return BadRequest(msg);
                }
                if (check.isValidCatalog_Ref_Id(items["Catalog_Ref_Id"].ToString()) != true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Catalog ref id incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Catalog ref id can't empty."}
                    };
                return BadRequest(msg);
            }


            if (!string.IsNullOrWhiteSpace(items["Availability_Status"].ToString()) &&
                items["Availability_Status"].ToString().Trim().Length !=0)
            {
                if (check.isValidAvailability_Status(items["Availability_Status"].ToString())!= true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Availability status incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Availability status can't empty."}
                    };
                return BadRequest(msg);
            }


            if (!string.IsNullOrWhiteSpace(items["Availability_Date"].ToString()) && 
                items["Availability_Date"].ToString().Trim().Length!=0)
            {
                DateTime myDateTime = DateTime.SpecifyKind(
                    DateTime.Parse(items["Availability_Date"].ToString()), 
                    DateTimeKind.Utc);
                string a = myDateTime.ToString("o");

                if (check.isValidAvailability_Date(a)!= true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Availability date incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Availability date can't empty."}
                    };
                return BadRequest(msg);
            }

            if (!string.IsNullOrWhiteSpace(items["Stock_Level"].ToString()) &&
                items["Stock_Level"].ToString().Trim().Length!= 0)
            {
                if (check.isValidStock_Level(items["Stock_Level"].ToString())!= true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Stock level incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Stock level can't empty."}
                    };
                return BadRequest(msg);
            }

            if (!string.IsNullOrWhiteSpace(items["Stock_Thresh"].ToString()) &&
                items["Stock_Thresh"].ToString().Trim().Length != 0)
            {
                if (check.isValidStock_Thresh(items["Stock_Thresh"].ToString())!= true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Stock Thresh inccorect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Stock Thres can't empty."}
                    };
                return BadRequest(msg);
            }

            if (!string.IsNullOrWhiteSpace(items["Product_Id"].ToString()))
            {
                if (items["Product_Id"].ToString().Trim().Length != 24)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Product id length is 24"}
                    };
                    return BadRequest(msg);
                }

                if (check.isValidProduct_Id(items["Product_Id"].ToString())!= true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Product id incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Product id can't empty."}
                    };
                return BadRequest(msg);
            }

            if (!string.IsNullOrWhiteSpace(items["Created_By"].ToString()))
            {
                if (items["Created_By"].ToString().Trim().Length != 24)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Created by id length is 24"}
                    };
                    return BadRequest(msg);
                }

                if (check.isValidCreated_By(items["Created_By"].ToString())!=true)
                {
                    msg = new Dictionary<string, string>()
                    {
                        {"msg","Created by incorrect."}
                    };
                    return BadRequest(msg);
                }
            }
            else
            {
                msg = new Dictionary<string, string>()
                    {
                        {"msg","Created by can't empty."}
                    };
                return BadRequest(msg);
            }

            InventoryItems item = new InventoryItems(
                int.Parse(items["Version"].ToString()),
                DateTime.Parse(items["Start_Date"].ToString()),
                DateTime.Parse(items["End_Date"].ToString()),
                items["Display_Name"].ToString(),
                items["Description"].ToString(),
                items["Catalog_Ref_Id"].ToString(),
                items["Availability_Status"].ToString(),
                DateTime.Parse(items["Availability_Date"].ToString()),
                int.Parse(items["Stock_Level"].ToString()),
                int.Parse(items["Stock_Thresh"].ToString()),
                items["Product_Id"].ToString(),
                items["Created_By"].ToString()
            ); 
            

            var inventoryItems = _services.AddInventoryItemsAsync(item);

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
