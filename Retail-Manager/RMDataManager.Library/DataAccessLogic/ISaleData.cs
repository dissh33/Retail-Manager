using System.Collections.Generic;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccessLogic
{
    public interface ISaleData
    {
        void SaveSale(SaleModel saleInfo, string userId);
        List<SaleReportModel> GetSaleReport();
    }
}