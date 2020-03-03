using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using RMDataManager.Library.Models.DbModels;

namespace RMDataManager.Library.DataAccessLogic
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string userId)
        {
            //TODO: Make this solid, dry / better
            

            List<DbSaleDetailModel> details = new List<DbSaleDetailModel>();
            ProductData products = new ProductData();

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new DbSaleDetailModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get the information about this product
                var productInfo = products.GetProductById(item.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {item.ProductId } could not be found in the database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                details.Add(detail);
            }

            // Create the Sale mode
            DbSaleModel sale = new DbSaleModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Discount = saleInfo.Discount,
                CashierId = userId
            };

            sale.Total = sale.SubTotal - sale.Discount;

            // Save the sale model
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData<DbSaleModel>("dbo.spSale_Insert", sale, "RMData");

            // Get id from sale model
            sale.Id = sql.LoadData<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }, "RMData").FirstOrDefault();

            decimal discountPercent = (sale.Discount * 100) / sale.SubTotal;

            // Finish filling in the sale detail models
            foreach (var item in details)
            {
                item.SaleId = sale.Id;
                item.Discount = (item.PurchasePrice/100) * discountPercent;

                //Save the sale details models
                sql.SaveData("dbo.spSaleDetail_Insert", item, "RMData");
            }
        }

    }
}