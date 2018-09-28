using System;
using Microsoft.AspNetCore.Mvc;
namespace InventoryService.Controllers
{
    public class ParameterRequest
    {
        [FromQuery(Name = "Id")]
        public string id { get; set; } = "";

        public ParameterRequest()
        {
        }
    }
}
