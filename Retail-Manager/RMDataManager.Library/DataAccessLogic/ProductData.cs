using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccessLogic
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sql;

        public ProductData(ISqlDataAccess sqlDataAccess)
        {
            _sql = sqlDataAccess;
        }

        public List<ProductModel> GetProducts()
        {
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "RMData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "RMData").FirstOrDefault();

            return output;
        }
    }
}
