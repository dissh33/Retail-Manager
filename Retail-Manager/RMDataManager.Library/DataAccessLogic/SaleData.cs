using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using RMDataManager.Library.Models.DbModels;

namespace RMDataManager.Library.DataAccessLogic
{
    public class SaleData : ISaleData
    {
        private readonly ISqlDataAccess _sql;
        private readonly IProductData _productData;

        public SaleData(ISqlDataAccess sqlDataAccess, IProductData productData)
        {
            _sql = sqlDataAccess;
            _productData = productData;
        }

        public void SaveSale(SaleModel saleInfo, string userId)
        {
            //TODO: Make this solid, dry / better
            
            List<DbSaleDetailModel> details = new List<DbSaleDetailModel>();

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new DbSaleDetailModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get the information about this product
                var productInfo = _productData.GetProductById(item.ProductId);

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
                try
                {
                    _sql.StartTransaction("RMData");

                    _sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    // Get id from sale model
                    sale.Id = _sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                    //Calculate Discount
                    decimal discountPercent = (sale.Discount * 100) / sale.SubTotal;

                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        item.Discount = (item.PurchasePrice / 100) * discountPercent;

                        //Save the sale details models
                        _sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    _sql.CommitTransaction();
                }
                catch (Exception)
                {
                    _sql.RollbackTransaction();
                    throw;
                }

                _sql?.Dispose();
        }


        public List<SaleReportModel> GetSaleReport()
        {
            var output = _sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "RMData");

            return output;
        }
    }
}