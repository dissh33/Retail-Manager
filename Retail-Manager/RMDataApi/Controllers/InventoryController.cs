using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMDataManager.Library.DataAccessLogic;
using RMDataManager.Library.Models;

namespace RMDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy="AdminRole,ManagerRole")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryData _inventoryData;

        public InventoryController(IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        [HttpGet]
        public List<InventoryModel> Get()
        {
            return _inventoryData.GetInventory();
        }

        [HttpPost]
        public void Post(InventoryModel item)
        {
            _inventoryData.SaveInventoryRecord(item);
        }
    }
}