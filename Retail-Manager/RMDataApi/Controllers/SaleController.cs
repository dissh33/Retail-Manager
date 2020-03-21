using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleData _saleData;

        public SaleController(ISaleData saleData)
        {
            _saleData = saleData;
        }

        [Authorize(Policy = "CashierRole")]
        [HttpPost]
        public void Post(SaleModel sale)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _saleData.SaveSale(sale, userId);
        }

        [Authorize(Policy = "AdminRole,ManagerRole")]
        [Route("GetSalesReport")] 
        [HttpGet]
        public List<SaleReportModel> GetSalesReport()
        {
            return _saleData.GetSaleReport();
        }
    }
}