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

            // Create the Sale model
            DbSaleModel sale = new DbSaleModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Discount = saleInfo.Discount,
                CashierId = userId
            };

            sale.Total = sale.SubTotal - sale.Discount;

            // Save the sale model in Transaction
            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("RMData");

                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    // Get id from sale model
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                    //Calculate Discount
                    decimal discountPercent = (sale.Discount * 100) / sale.SubTotal;

                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        item.Discount = (item.PurchasePrice / 100) * discountPercent;

                        //Save the sale details models
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    sql.CommitTransaction();
                }
                catch (Exception)
                {
                    sql.RollbackTransaction();
                    throw;
                }
            }
        }

    }
}